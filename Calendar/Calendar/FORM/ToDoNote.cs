using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class ToDoNote : Form
    {
        public int IdNote = 0;
        public int Check;

        public ToDoNote()
        {
            InitializeComponent();
        }
        private void ToDoNote_Load_1(object sender, EventArgs e)
        {
            RepeatCB.SelectedIndex = 0;
            if (string.IsNullOrWhiteSpace(FHoursCB.Text))
            {
                THoursCB.SelectedIndex = 0;
                TMinutesCB.SelectedIndex = 0;
                FHoursCB.SelectedIndex = 0;
                FMinutesCB.SelectedIndex = 0;
            }
            try
            {
                NoteData.Connect();
            }
            catch
            {
                MessageBox.Show("Lỗi hiển thị. Vui lòng thử lại sau.", "Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ToDtpk.Value = FromDtpk.Value;
        }

        private void SaveBtn_Click_1(object sender, EventArgs e)
        {
            if (MainNote.Text == "Nhập ghi chú")
            {
                MessageBox.Show("Ghi chú không được rỗng!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DescriptionTB.Text == "Mô tả")
                DescriptionTB.Text = null;
            if ((Convert.ToInt32(FHoursCB.Text) > Convert.ToInt32(THoursCB.Text)) || ((Convert.ToInt32(FHoursCB.Text) == Convert.ToInt32(THoursCB.Text)) && (Convert.ToInt32(FMinutesCB.Text) > Convert.ToInt32(TMinutesCB.Text))) || FromDtpk.Value > ToDtpk.Value)
            {
                MessageBox.Show("Đặt thời gian không hợp lệ!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ImportantCheck.Checked == true) Check = 1;
            else Check = 0;
            string sql;

            if (NoteData.IsConfirm("Bạn có muốn lưu lại?"))
            {
                FromDtpk.Value = new DateTime(FromDtpk.Value.Year, FromDtpk.Value.Month, FromDtpk.Value.Day, 0, 0, 0);
                ToDtpk.Value = new DateTime(ToDtpk.Value.Year, ToDtpk.Value.Month, ToDtpk.Value.Day, 0, 0, 0);
                if (IdNote == 0)
                {

                    if (RepeatCB.SelectedIndex == 0)
                    {
                        FromDtpk.Value.AddSeconds(1);
                        sql = $"insert into NoteByDate(NoteText,Description,AppDate,FromH,FromM,ToH,ToM,Important) " +
                           $"values('{MainNote.Text}','{DescriptionTB.Text}','{FromDtpk.Value}','{FHoursCB.Text}','{FMinutesCB.Text}','{THoursCB.Text}','{TMinutesCB.Text}','{Check}')";
                        NoteData.UpdateInsertDelete(sql);
                    }
                    else if (RepeatCB.SelectedIndex == 1)
                    {
                        while (FromDtpk.Value <= ToDtpk.Value)
                        {
                            sql = $"insert into NoteByDate(NoteText,Description,AppDate,FromH,FromM,ToH,ToM,Important) " +
                           $"values('{MainNote.Text}','{DescriptionTB.Text}','{FromDtpk.Value}','{FHoursCB.Text}','{FMinutesCB.Text}','{THoursCB.Text}','{TMinutesCB.Text}','{Check}')";
                            NoteData.UpdateInsertDelete(sql);
                            FromDtpk.Value = FromDtpk.Value.AddDays(1);
                        }
                    }
                    else if (RepeatCB.SelectedIndex == 2)
                    {
                        while (FromDtpk.Value <= ToDtpk.Value)
                        {
                            sql = $"insert into NoteByDate(NoteText,Description,AppDate,FromH,FromM,ToH,ToM,Important)" +
                           $"values('{MainNote.Text}','{DescriptionTB.Text}','{FromDtpk.Value}','{FHoursCB.Text}','{FMinutesCB.Text}','{THoursCB.Text}','{TMinutesCB.Text}','{Check}')";
                            NoteData.UpdateInsertDelete(sql);
                            FromDtpk.Value = FromDtpk.Value.AddDays(7);
                        }
                    }
                    else if (RepeatCB.SelectedIndex == 3)
                    {
                        while (FromDtpk.Value <= ToDtpk.Value)
                        {
                            sql = $"insert into NoteByDate(NoteText,Description,AppDate,FromH,FromM,ToH,ToM,Important) " +
                           $"values('{MainNote.Text}','{DescriptionTB.Text}','{FromDtpk.Value}','{FHoursCB.Text}','{FMinutesCB.Text}','{THoursCB.Text}','{TMinutesCB.Text}','{Check}')";
                            NoteData.UpdateInsertDelete(sql);
                            FromDtpk.Value = FromDtpk.Value.AddMonths(1);
                        }
                    }
                    else if (RepeatCB.SelectedIndex == 4)
                    {
                        while (FromDtpk.Value <= ToDtpk.Value)
                        {
                            sql = $"insert into NoteByDate(NoteText,Description,AppDate,FromH,FromM,ToH,ToM,Important) " +
                           $"values('{MainNote.Text}','{DescriptionTB.Text}','{FromDtpk.Value}','{FHoursCB.Text}','{FMinutesCB.Text}','{THoursCB.Text}','{TMinutesCB.Text}','{Check}')";
                            NoteData.UpdateInsertDelete(sql);
                            FromDtpk.Value = FromDtpk.Value.AddYears(1);
                        }
                    }
                }
                else
                {
                    sql = $"update NoteByDate set NoteText = '{MainNote.Text}', Description = '{DescriptionTB.Text}', AppDate = '{FromDtpk.Value}', FromH = '{FHoursCB.Text}', FromM = '{FMinutesCB.Text}', ToH = '{THoursCB.Text}', ToM = '{TMinutesCB.Text}', Important = '{Check}' where IdNote = {IdNote}";
                    NoteData.UpdateInsertDelete(sql);
                }
                MessageBox.Show("Lưu dữ liệu thành công.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void RepeatCB_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (RepeatCB.Text == "Không lặp")
            {
                ToDate.Visible = false;
                ToDtpk.Visible = false;
            }
            else
            {
                ToDate.Visible = true;
                ToDtpk.Visible = true;
            }
        }

        private void ExitBtn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainNote_Enter_1(object sender, EventArgs e)
        {
            if (MainNote.Text == "Nhập ghi chú")
            {
                MainNote.Text = "";
                MainNote.ForeColor = Color.Black;
            }
        }

        private void MainNote_Leave_1(object sender, EventArgs e)
        {
            if (MainNote.Text == "")
            {
                MainNote.Text = "Nhập ghi chú";
                MainNote.ForeColor = Color.LightGray;
            }
        }

        private void DescriptionTB_Enter_1(object sender, EventArgs e)
        {
            if (DescriptionTB.Text == "Mô tả")
            {
                DescriptionTB.Text = "";
                DescriptionTB.ForeColor = Color.Black;
            }
        }

        private void DescriptionTB_Leave_1(object sender, EventArgs e)
        {
            if (DescriptionTB.Text == "")
            {
                DescriptionTB.Text = "Mô tả";
                DescriptionTB.ForeColor = Color.LightGray;
            }
        }

        private void FromDtpk_ValueChanged(object sender, EventArgs e)
        {
            if (RepeatCB.SelectedIndex == 0)
            {
                ToDtpk.Value = FromDtpk.Value;
            }
        }
    }
}
