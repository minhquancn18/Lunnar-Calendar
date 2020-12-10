﻿using LunnarSample;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Calendar
{
    public partial class DisplayWF : UserControl
    {
        private static DisplayWF WFdisplay;
        public DisplayWF()
        {
            InitializeComponent();
        }
        public static DisplayWF WFDisplay
        {
            get
            {
                if (WFdisplay == null)
                    WFdisplay = new DisplayWF();
                return WFdisplay;
            }
        }

        //Thông tin của API openweathermap để lấy thoong tin của dự báo thời tiết
        //http://home.openweathermap.org/users/sign_in 
        private const string API_KEY = "8ffb7eed5005c82c5024719a263f9935";

        #region Chuyển đổi từ milisecond sang ngày
        DateTime getDate(double millisecound)
        {

            DateTime day = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisecound).ToLocalTime();

            return day;
        }
        #endregion

        #region Tìm thông tin thời tiết
        void GetWeather()
        {
            StringText sText = new StringText();
            sText.GetList();
            string cityID = sText.IsEqualCityName(ComboBoxCity.Text);
            string url = string.Format(
                    "http://api.openweathermap.org/data/2.5/weather?id={0}&units=metric&cnt=6&appid={1}"
                    , cityID, API_KEY);
            using (WebClient web = new WebClient())
            {
                try
                {
                    var json = web.DownloadString(url);
                    var result = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                    WeatherInfo.root output = result;
                    string urlicon = string.Format("http://openweathermap.org/img/wn/{0}@2x.png", output.weather[0].icon);
                    byte[] image = web.DownloadData(urlicon);
                    MemoryStream stream = new MemoryStream(image);
                    Bitmap newbitmap = new Bitmap(stream);
                    Bitmap icon = newbitmap;
                    PictureWeather.Image = newbitmap;
                    labelCity.Text = ComboBoxCity.Text;
                    labelCountry.Text = "Việt Nam";
                    labelLog.Text = string.Format("{0}\u00B0", Math.Round(output.coord.lon, 1));
                    labelLat.Text = string.Format("{0}\u00B0", Math.Round(output.coord.lat, 1));
                    labelTemp.Text = string.Format("{0}\u00B0" + "C", Math.Round(output.main.temp, 0));
                    if (Math.Round(output.main.temp_min, 0) == Math.Round(output.main.temp_max, 0))
                        labelTempMinMax.Text = string.Format("{0}\u00B0" +"C", Math.Round(output.main.temp_max, 0));
                    else
                        labelTempMinMax.Text = string.Format("{0}\u00B0" + "C/{1}\u00B0" + "C", Math.Round(output.main.temp_min, 0), Math.Round(output.main.temp_max, 0));
                    labelWeather.Text = sText.IsEqualMainWeather(output.weather[0].main);
                    labelCloud.Text = sText.IsEqualDescriptionWeather(output.weather[0].description);
                    labelHumidity.Text = string.Format("{0}%", output.main.humidity);
                    labelWind.Text = string.Format("{0} km/h", output.wind.speed);
                    labelPressure.Text = string.Format("{0} hPa", output.main.pressure);
                    labelVisible.Text = string.Format("{0} km",output.visibility / 1000);
                    labelDate.Text = string.Format("NGÀY {0}/{1}/{2}"
                        , getDate(output.dt).Day, getDate(output.dt).Month, getDate(output.dt).Year);
                    labelHour.Text = string.Format("Giờ {0}", getDate(output.dt).ToShortTimeString());
                }
                catch (WebException we)
                {
                    try
                    {
                        StreamReader reader = new StreamReader(we.Response.GetResponseStream());
                        XmlDocument response_doc = new XmlDocument();
                        response_doc.LoadXml(reader.ReadToEnd());
                        XmlNode message_node = response_doc.SelectSingleNode("//message");
                        MessageBox.Show(message_node.InnerText);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unknown error\n" + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unknown error\n" + ex.Message);
                }
            }
        }
        #endregion

        #region Sự kiện của button tìm kiếm
        private void ButtonFind_Click(object sender, EventArgs e)
        {
            PanelDisplayWT.Visible = true;
            GetWeather();
        }
        #endregion
    }
}