using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Diagnostics;

namespace DeleteSameNameFiles2._0
{   
    public partial class Form1 : Form
    {
        DataTable dt_filelistinfo1 = null;
        DataTable dt_filelistinfo2 = null;
        DataTable dt_dupfilelist = null;
        DataTable dt_diffilelist = null;
        int flag_btn1=0;
        int flag_btn2=0;
        int flag_dup_btn=0;
        int flag_dif_btn=0;
        String strFilePath1=null;
        String strFilePath2=null;

        public Form1()
        {
            InitializeComponent();
            //groupBox1.BackColor = Color.Transparent;
            //groupBox2.BackColor = Color.Transparent;
            //groupBox3.BackColor = Color.Transparent;
           // groupBox4.BackColor = Color.Transparent;
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = true;

            strFilePath1 = null;
            flag_btn1=1;
            // CommonOpenFileDialog
            CommonOpenFileDialog fileDialog1 = new CommonOpenFileDialog();

            // 처음 보여줄 폴더 설정
            fileDialog1.InitialDirectory = "%systemdrive%\\users\\%username%\\desktop";
            fileDialog1.IsFolderPicker = true;
            if (fileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox1.Text = fileDialog1.FileName; // 선택한 폴더 이름 출력
                strFilePath1 = fileDialog1.FileName;
                dt_filelistinfo1 = GetFileListFromFolderPath(fileDialog1.FileName);
                ShowDataFromDataTableToDataGridView(dt_filelistinfo1, dataGridView1);
            }

            if (btnFindDup.Enabled == false && flag_btn1 != 0 && flag_btn2 != 0)
                btnFindDup.Enabled = true;

            if (btnFindDif.Enabled == false && flag_btn1 != 0 && flag_btn2 != 0)
                btnFindDif.Enabled = true;

            if(dt_filelistinfo1 != null)
                label1.Text = "총" + dt_filelistinfo1.Rows.Count + "개의 파일 불러옴";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = true;

            strFilePath2 = null;
            flag_btn2 =1;
            // CommonOpenFileDialog
            CommonOpenFileDialog fileDialog2 = new CommonOpenFileDialog();

            // 처음 보여줄 폴더 설정
            fileDialog2.InitialDirectory = "%systemdrive%\\users\\%username%\\desktop";
            fileDialog2.IsFolderPicker = true;
            if (fileDialog2.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox2.Text = fileDialog2.FileName;
                strFilePath2 = fileDialog2.FileName;
                dt_filelistinfo2 = GetFileListFromFolderPath(fileDialog2.FileName);
                ShowDataFromDataTableToDataGridView(dt_filelistinfo2, dataGridView2);
            }

            if (btnFindDup.Enabled == false && flag_btn1 != 0 && flag_btn2 != 0)
                btnFindDup.Enabled = true;


            if (btnFindDif.Enabled == false && flag_btn1 != 0 && flag_btn2 != 0)
                btnFindDif.Enabled = true;

            if (dt_filelistinfo2 != null)
                label7.Text = "총" + dt_filelistinfo2.Rows.Count + "개의 파일 불러옴";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //체크박스 1
            if (checkBox1.Checked == true)
                checkBox2.Checked = false;
            else
                checkBox2.Checked = true;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // 체크박스2 
            // 얘 건들면 체크박스1 체크 해제
            if(checkBox2.Checked == true)
                checkBox1.Checked = false;
            else
                checkBox1.Checked = true;
        }

        private void btnFindDif_Click(object sender, EventArgs e)
        {
            flag_dif_btn = 1;
            flag_dup_btn = 0;

            // 목록 색칠을 위해 다시 그리기
            ShowDataFromDataTableToDataGridView(dt_filelistinfo1, dataGridView1);
            ShowDataFromDataTableToDataGridView(dt_filelistinfo2, dataGridView2);

            // 선택박스 체크 안되어 있으면 선택하라고 하기
            if (checkBox1.Checked == false && checkBox2.Checked == false)
                MessageBox.Show("기준 폴더를 선택하세요.");
            else
            {
                // 체크박스 선택된 폴더를 기준으로 한다
                if (checkBox1.Checked == true)
                    dt_diffilelist = FindDifFile(dt_filelistinfo1, dt_filelistinfo2, dataGridView1, dataGridView2);
                else
                    dt_diffilelist = FindDifFile(dt_filelistinfo2, dt_filelistinfo1, dataGridView2, dataGridView1);

                // 비교 버튼 누르고 나서야 삭제 버튼 활성화
                btnDelete.Enabled = true;

                // 안 중복 목록 표시
                ShowDataFromDataTableToDataGridView(dt_diffilelist, dataGridView3);
            }

        }

        private void btnFindDup_Click(object sender, EventArgs e)
        {
            flag_dup_btn = 1;
            flag_dif_btn = 0;

            // 목록 색칠을 위해 다시 그리기
            ShowDataFromDataTableToDataGridView(dt_filelistinfo1, dataGridView1);
            ShowDataFromDataTableToDataGridView(dt_filelistinfo2, dataGridView2);

            // 선택박스 체크 안되어 있으면 선택하라고 하기
            if (checkBox1.Checked == false && checkBox2.Checked == false)
                MessageBox.Show("기준 폴더를 선택하세요.");
            else
            {
                // 체크박스 선택된 폴더를 기준으로 한다
                if (checkBox1.Checked == true)
                    dt_dupfilelist = FindDupFile(dt_filelistinfo1, dt_filelistinfo2, dataGridView1, dataGridView2);
                else
                    dt_dupfilelist = FindDupFile(dt_filelistinfo2, dt_filelistinfo1, dataGridView2, dataGridView1);
               
                // 비교 버튼 누르고 나서야 삭제 버튼 활성화
                btnDelete.Enabled = true;

                //중복 목록 표시
                ShowDataFromDataTableToDataGridView(dt_dupfilelist, dataGridView3);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("확인 버튼을 누르면 파일이 휴지통으로 이동합니다.\r계속 하시겠습니까?", "파일 삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Abort;
                // 중복 파일 비교 시
                if (flag_dup_btn == 1)
                {
                    // 중복 파일이 없으면 삭제하지 않는다
                    if (dt_dupfilelist == null)
                        MessageBox.Show("삭제할 파일이 없습니다.");
                    else
                        DeleteFileName(dt_dupfilelist);
                }
                // 중복되지 않는 파일 비교 시
                else if (flag_dif_btn == 1)
                {
                    if (dt_diffilelist == null)
                        MessageBox.Show("삭제할 파일이 없습니다.");
                    else
                        DeleteFileName(dt_diffilelist);
                }

                //삭제하고 나서 파일의 리스트를 업데이트
                dt_dupfilelist = null;
                dt_diffilelist = null;
                dt_filelistinfo1 = GetFileListFromFolderPath(strFilePath1);
                dt_filelistinfo2 = GetFileListFromFolderPath(strFilePath2);
                ShowDataFromDataTableToDataGridView(dt_filelistinfo1, dataGridView1);
                ShowDataFromDataTableToDataGridView(dt_filelistinfo2, dataGridView2);
                ShowDataFromDataTableToDataGridView(dt_dupfilelist, dataGridView3);
                label3.Text = "파일 없음";
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        // 중복되지 않는 파일 확인 함수 - 기준 폴더 리스트에서 중복 리스트 제외
        private DataTable FindDifFile(DataTable dt1, DataTable dt2, DataGridView dgv1, DataGridView dgv2)
        {

            int cnt_row1 = 0; // 데이터 그리드에 같은 값 표시하기 위한 카운트 체크
            int cnt_row2 = 0; // 데이터 그리드에 같은 값 표시하기 위한 카운트 체크
            int cnt_equal = 0;

            DataGridViewRow dgv_row1;
            DataGridViewRow dgv_row2;

            DataTable dt3 = new DataTable();
            dt3.Columns.Add("fullPath", typeof(string)); // dt1을 기준으로 중복 파일의 전체 경로 저장

            foreach (DataRow dr1 in dt1.Rows)
            {
                dgv_row1 = dgv1.Rows[cnt_row1];
                foreach (DataRow dr2 in dt2.Rows)
                {
                    dgv_row2 = dgv2.Rows[cnt_row2];

                    //중복 찾으면 중복 카운트 올림
                    if (Equals(dr1[dt1.Columns[2]], dr2[dt2.Columns[2]]))
                        cnt_equal++;
                    cnt_row2++;
                }
                // 중복된게 하나도 없어야 dt_diffilelist에 저장
                if (cnt_equal == 0)
                {
                    dt3.Rows.Add(dr1[3].ToString());
                    dgv_row1.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                cnt_equal = 0;
                cnt_row2 = 0;
                cnt_row1++;
            }

            dt3 = dt3.DefaultView.ToTable(true);

            label3.Text = "선택한 폴더를 기준으로 " +dt1.Rows.Count +"개의 파일 중에서";
            label8.Text = "중복되지 않는 파일은 " + dt3.Rows.Count + "개 입니다.";
            return dt3;

        }

        // 중복 파일 확인 함수 - 새 데이터 테이블 생성
        // 동시에 그리드 뷰에 중복되는 것 하이라이트로 색칠
        private DataTable FindDupFile(DataTable dt1, DataTable dt2, DataGridView dgv1, DataGridView dgv2)
        {

            int cnt_same = 0;
            int cnt_row1 = 0;
            int cnt_row2 = 0;

            DataGridViewRow dgv_row1;
            DataGridViewRow dgv_row2;

            DataTable dt3 = new DataTable();
            dt3.Columns.Add("fullPath", typeof(string)); // dt1을 기준으로 중복 파일의 전체 경로 저장

            // 1이 기준으로 검색
            foreach (DataRow dr1 in dt1.Rows)
            {
                dgv_row1 = dgv1.Rows[cnt_row1];
                
                foreach (DataRow dr2 in dt2.Rows)
                {
                    dgv_row2 = dgv2.Rows[cnt_row2];
                    // 확장자 제외한 두 이름이가 같으면
                    if (Equals(dr1[dt1.Columns[2]], dr2[dt2.Columns[2]]))
                    {
                        //dt1이 기준 - dt3에 생성 - 전체 경로 저장
                        dt3.Rows.Add(dr1[3].ToString());
                        dgv_row1.DefaultCellStyle.BackColor = Color.Gainsboro;
                        dgv_row2.DefaultCellStyle.BackColor = Color.Gainsboro;
                        cnt_same++;
                    }                
                    cnt_row2++;
                }
                cnt_row2 = 0;
                cnt_row1++;
            }

            label3.Text = "선택한 폴더를 기준으로 " + dt1.Rows.Count + "개의 파일 중에서";
            label8.Text = "중복되는 파일은 " + dt3.Rows.Count + "개 입니다.";
            return dt3;

        }

        // 초기화 버튼
        private void btnReset_Click(object sender, EventArgs e)
        {
            Listinitialize();
            FormInitialize();
        }

        private void DeleteFileName(DataTable dt1)
        {
            // 파일 휴지통으로 보내는 함수
            foreach (DataRow dr1 in dt1.Rows)
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(dr1[0].ToString(),
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
            }
        }

        /// 선택한 폴더의 파일 목록을 DataTable형식으로 내보냅니다.
        /// 전역 변수 데이터 테이블 두개에 할당하는 함수임
        /// FolderName : 선택한 폴더의 전체 경로를 입력
        private DataTable GetFileListFromFolderPath(string FolderName)
        {
            DirectoryInfo di = new DirectoryInfo(FolderName); // 해당 폴더 정보를 가져옵니다.
            DataTable dt1 = new DataTable(); // 새로운 테이블 작성합니다.(FileInfo 에서 가져오기 원하는 속성을 열로 추가합니다.) 
            dt1.Columns.Add("FileName", typeof(string)); // 파일의 이름(확장자 포함)
            dt1.Columns.Add("DirectoryName", typeof(string)); // 폴더 경로 이름
            dt1.Columns.Add("FileNameOnly", typeof(string));    //확장자 제외한 파일 이름
            dt1.Columns.Add("FullPath", typeof(string)); // 전체 경로

            foreach (FileInfo File in di.GetFiles()) // 선택 폴더의 파일 목록을 스캔합니다. 
            {
                String filePath = @File.Name;
                dt1.Rows.Add(File.Name, File.DirectoryName, Path.GetFileNameWithoutExtension(filePath), File.FullName);
                filePath = null;
            }

            return dt1;
        }

        /// 선택한 폴더의 파일 목록을 가져와서 DataGridView 도구에 보여줍니다. 
        /// dt:선택한 폴더의 파일 목록이 들어있는 DataTable을 입력
        /// dgv1:결과를 출력할 DataGridView를 선택
        private void ShowDataFromDataTableToDataGridView(DataTable dt1, DataGridView dgv1)
        {
            dgv1.Rows.Clear(); // 이전 정보가 있을 경우, 모든 행을 삭제합니다. 
            dgv1.Columns.Clear(); // 이전 정보가 있을 경우, 모든 열을 삭제합니다. 


            // 열 전부 출력
            /*
            foreach (DataColumn dc1 in dt1.Columns) // 선택한 파일 목록이 들어있는 DataTable의 모든 열을 스캔합니다. 
            {
                dgv1.Columns.Add(dc1.ColumnName, dc1.ColumnName); // 출력할 DataGridView에 열을 추가합니다. 
            }
            */

            // 첫번째 열 한개만 출력
            if (dt1 != null)
            {
                dgv1.Columns.Add(dt1.Columns[0].ColumnName, dt1.Columns[0].ColumnName);

                int row_index = 0; // 행 인덱스 번호(초기 값) 
                foreach (DataRow dr1 in dt1.Rows) // 선택한 파일 목록이 들어있는 DataTable의 모든 행을 스캔합니다. 
                {
                    dgv1.Rows.Add(); // 빈 행을 하나 추가합니다. 
                    dgv1.Rows[row_index].Cells[dt1.Columns[0].ColumnName].Value = dr1[dt1.Columns[0].ColumnName];
                    /*
                    foreach (DataColumn dc1 in dt1.Columns) // 선택한 파일 목록이 들어있는 DataTable의 모든 열을 스캔합니다. 
                    {
                        dgv1.Rows[row_index].Cells[dc1.ColumnName].Value = dr1[dc1.ColumnName]; // 선택 행 별로, 스캔하는 열에 해당하는 셀 값을 입력합니다. 
                    }
                    */
                    row_index++; // 다음 행 인덱스를 선택하기 위해 1을 더해줍니다. 
                }

                foreach (DataGridViewColumn drvc1 in dgv1.Columns) // 결과를 출력할 DataGridView의 모든 열을 스캔합니다. 
                {
                    drvc1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // 선택 열의 너비를 자동으로 설정합니다. 
                }
            }
        }

        private void FormInitialize()
        {
            textBox1.Text = "";
            textBox2.Text = "";

            checkBox1.Checked = false;
            checkBox2.Checked = false;

            label1.Text = "파일 없음";
            label3.Text = "기준 폴더 없음";
            label8.Text = "파일 없음";
            label7.Text = "파일 없음";

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
        }

        private void Listinitialize()
        {
            dt_filelistinfo1 = null;
            dt_filelistinfo2 = null;
            dt_diffilelist = null;
            dt_dupfilelist = null;

            flag_btn1 = 0;
            flag_btn2 = 0;
            flag_dup_btn = 0;
            flag_dif_btn = 0;

            strFilePath1 = null;
            strFilePath2 = null;
        }

    }

}
