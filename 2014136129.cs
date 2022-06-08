using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;      //serial 포트를 사용하기위해
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Consumer_Producer
{
    public partial class producerAndCustomer : Form
    {
      
        int idx_c = 0;  //소비자 원형 버퍼 접근 인덱스
        int idx_p = 0;  //생산자 원형 버퍼 접근 인덱스
        int i_xP = 267; //생산자에서 출발하는 트럭의 x 축 시작 위치 
        int i_xC = 743; //원형버퍼에서 출발하는 트럭의 x축 시작 위치
        int i_y = 318;  //트럭의 y축 시작 위치 

       
        Button[] BT_Pro = new Button[5];    //생산자 버튼 배열 저장
        Button[] BT_Cus = new Button[5];    //소비자 버튼 배열 저장

        TextBox[] TB_Pro = new TextBox[5];  //생산자 생성 시간 입력 텍스트 박스 배열
        TextBox[] TB_Cus = new TextBox[5];  //소비자 생성 시간 입력 텍스트 박스 배열

        PictureBox[] pArr = new PictureBox[16]; //원형버퍼에 출력할 이미지 배열
        Boolean[] enImg = new Boolean[16];      //이미지 출력 여부 저장 배열

        Boolean []b_Pclick = new Boolean[5];    //생산자 버튼 클릭 여부 저장 배열   
        Boolean []b_Cclick = new Boolean[5];    //소비자 버튼 클릭 여부 저장 배열   
        Timer[] tm_Parr = new Timer[5];     //생산자 타이머 배열
        Timer[] tm_Carr = new Timer[5];     //소비자 타이머 배열


        PictureBox[] truck_P = new PictureBox[5];   //생산자 트럭 이미지 배열
        PictureBox[] truck_C = new PictureBox[5];   //소비자 트럭 이미지 배열
        int[] cnt_P_arr = new int[5];   //생산자 트럭 이동 위치 저장 배열
        int[] cnt_C_arr = new int[5];   //소비자 트럭 이동 위치 저장 배열


        Boolean[] b_PTruck = new Boolean[5];    //생산자 트럭 이미지 출력 여부 저장 배열
        Boolean[] b_CTruck = new Boolean[5];    //소비자 트럭 이미지 출력 여부 저장 배열
        public producerAndCustomer()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {

                cnt_P_arr[i] = 0;   //생산자 트럭 이동 위치 저장 배열 초기화
                cnt_C_arr[i] = 0;   //소비자 트럭 이동 위치 저장 배열 초기화
                b_PTruck[i] = false;    //생산자 버튼 클릭 여부 저장 배열 초기화
                b_CTruck[i] = false;    //소비자 버튼 클릭 여부 저장 배열 초기화

                //생산자 트럭 이미지 배열 초기화(컨트롤 연결)
                truck_P[i] = ((PictureBox)this.Controls[("truck" + (i)).ToString()]);
                truck_P[i].Visible = false;                
                truck_P[i].SetBounds(i_xP, i_y, truck_P[i].Width, truck_P[i].Height);

                //소비자 트럭 이미지 배열 초기화(컨트롤 연결)
                truck_C[i] = ((PictureBox)this.Controls[("truck" + (i + 5)).ToString()]);
                truck_C[i].Visible = false;
                truck_C[i].SetBounds(i_xC, i_y, truck_C[i].Width, truck_C[i].Height);

              

                //생산자 및 소비자 시간 설정용 텍스트 박스 배열 초기화(컨트롤 연결)
                TB_Pro[i] = ((TextBox)this.Controls[("textBox" + (i + 1)).ToString()]);
                TB_Cus[i] = ((TextBox)this.Controls[("textBox" + (i + 6)).ToString()]);

                //각 텍스트 박스 출력 값 0 으로 초기화
                TB_Pro[i].Text = "0";
                TB_Cus[i].Text = "0";

                //생산자 및 소비자 버튼 배열 초기화(컨트롤 연결)
                BT_Pro[i] = ((Button)this.Controls[("BT_p" + (i + 1)).ToString()]);
                BT_Cus[i] = ((Button)this.Controls[("BT_c" + (i + 1)).ToString()]);
         
                //버튼에 글자 추가
                BT_Pro[i].Text = (i+1).ToString();
                BT_Cus[i].Text = (i + 1).ToString();

                //버튼 배열 클릭 핸들러 등록
                BT_Pro[i].Click += new EventHandler(Form1_Pro_BT);
                BT_Pro[i].Tag = i;
                BT_Cus[i].Click += new EventHandler(Form1_Cus_BT);
                BT_Cus[i].Tag = i;

                //생산자별 타이머 생성 및 핸들러 등록
                tm_Parr[i] = new Timer();
                tm_Parr[i].Interval = 1000;
                tm_Parr[i].Stop();
                tm_Parr[i].Tag = i;
                tm_Parr[i].Tick += new EventHandler(Form1_Pro_tick);

                //소비자별 타이머 생성 및 핸들러 등록
                tm_Carr[i] = new Timer();
                tm_Carr[i].Interval = 1000;
                tm_Carr[i].Stop();
                tm_Carr[i].Tag = i;
                tm_Carr[i].Tick += new EventHandler(Form1_Cus_tick);

                //클럭 여부 저장 배열 초기화
                b_Pclick[i] = false;
                b_Cclick[i] = false;
            }

            for (int i = 0; i < 16; i++)
            {
                //원형 버퍼 이미지 저장 배열 초기화(컨트롤 연결)
                pArr[i] = ((PictureBox)this.Controls[("pictureBox" + (i + 1)).ToString()]);
                pArr[i].Visible = false;
                enImg[i] = false;

            }

            //더블 버퍼링 설정
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        bool isEmpty() {    //원형 버퍼 비어있는지 검사
            return idx_p == idx_c;
        }
        bool isFull()       //원형 버퍼가 가득 차 있는지 검사
        {
            return idx_c == (idx_p+1)%16;
        }

        private void timer1_Tick(object sender, EventArgs e)    //트럭 이미지 애니메이션을 위한 타이머 헨들러
        {
            for (int i = 0; i < 5; i++) {   //생산자, 소비자가 각 5개가 최대 이기 때문에, 5번 반복문을 돌면서
                if (b_PTruck[i])    //생산자 트럭 애니메이션 생성이 enable 되었으면
                {
                    //enable된 트럭의 위치를 이동시켜주고
                    cnt_P_arr[i] += 15; 
                    truck_P[i].SetBounds(i_xP + cnt_P_arr[i], i_y, truck_P[i].Width, truck_P[i].Height);
                    //목적지에 도달하였으면,
                    if (cnt_P_arr[i] >= 120)
                    {
                        //버퍼에 상자 이미지를 출력해준다.
                        for (int j = 0; j < 16; j++)
                        {
                            pArr[j].Visible = enImg[j];
                        }
                        
                        b_PTruck[i] = false;        //트럭 애니메이션 생성 여부를 false로 설정해주고(애니메이션 종료)
                        truck_P[i].Visible = false; //도착을 완료 했기 때문에 트럭이미지를 보이지 않게 설정해준다.
                        cnt_P_arr[i] = 0;           //이동 카운트를 다시 초기화 해준다.

                       
                    }
                }
                else {
                    cnt_P_arr[i] = 0;                                                       //트럭 이미지 생성 변수가 disable로 설정 되어 있으면,
                    truck_P[i].SetBounds(i_xP, i_y, truck_P[i].Width, truck_P[i].Height);   //위치를 초기 위치로 설정해주고,
                    truck_P[i].Visible = false;                                             //이미지를 보이지 않게 설정한다.

                }

                if (b_CTruck[i])     //소비자 트럭 애니메이션 생성이 enable 되었으면
                {
                    //enable된 트럭의 위치를 이동시켜주고
                    cnt_C_arr[i] += 15;
                    truck_C[i].SetBounds(i_xC + cnt_C_arr[i], i_y, truck_C[i].Width, truck_C[i].Height);
                    if (cnt_C_arr[i] >= 120)    //목적지에 도달하였으면,
                    {                        
                        b_CTruck[i] = false;            //트럭 애니메이션 생성 여부를 false로 설정해주고(애니메이션 종료)
                        truck_C[i].Visible = false;     //도착을 완료 했기 때문에 트럭이미지를 보이지 않게 설정해준다.
                        cnt_C_arr[i] = 0;               //이동 카운트를 다시 초기화 해준다.
                    }
                }
                else
                {
                    cnt_C_arr[i] = 0;                                                       //트럭 이미지 생성 변수가 disable로 설정 되어 있으면,
                    truck_C[i].SetBounds(i_xC, i_y, truck_C[i].Width, truck_C[i].Height);   //위치를 초기 위치로 설정해주고,
                    truck_C[i].Visible = false;                                             //이미지를 보이지 않게 설정한다.

                }
            }
            
        }
        void Form1_Pro_tick(Object sender, EventArgs e) {   //생산자 타이머 처리 핸들러
               
            int t = Convert.ToInt32(((Timer)sender).Tag.ToString());    //태그를 읽어 해당 생산자 번호를 t 에 저장

            if(!isFull()) {                  //현재 원형 버퍼가 가득차 있지 않다면
                idx_p = (idx_p+1)%16;        //입력 인덱스 값을 증가 하고
                enImg[idx_p] = true;         //버퍼 이미지 enable을 true로 설정해준다
                b_PTruck[t] = true;          //트럭 애니메이션을 시작하기 위한 변수를 true로 설정
                truck_P[t].Visible = true;   //트럭 이미지를 보이게 설정함             
            }
        }
        void Form1_Cus_tick(Object sender, EventArgs e)     //소비자 타이머 처리 핸들러
        {
            int t = Convert.ToInt32(((Timer)sender).Tag.ToString());     //태그를 읽어 해당 소비자 번호를 t 에 저장
            if (!isEmpty())
            {                                    //현재 원형 버퍼가 비어 있지 않다면
                idx_c = (idx_c + 1) % 16;        //출력 인덱스 값을 증가 하고
                enImg[idx_c] = false;            //버퍼 이미지 enable을 false로 설정해준다
                pArr[idx_c].Visible = false;     //버퍼 이미지를 보이지 않게 설정하고
                b_CTruck[t] = true;             //트럭 애니메이션을 시작하기 위한 변수를  true로 설정          
                truck_C[t].Visible = true;      //트럭 이미지를 보이게 설정함
            }
        }
        void Form1_Pro_BT(Object sender, EventArgs e)   //생산자별 버튼 클릭시 처리하는 핸들러 함수
        {            
            int t = Convert.ToInt32(((Button)sender).Tag.ToString());//태그를 읽어와서 어떤 버튼이 눌렀는지 파별함
            int time = 0;       //타이머 시간 저장 임시 변수
            if (b_Pclick[t]) // 버튼이 눌렸었다면(생산자가 생산하고 있는 상태)
            {
                BT_Pro[t].BackColor = Color.Transparent;    //버튼의 색을 투명으로 변환
                b_Pclick[t] = false;                        //버튼 해제 정보를 저장
                TB_Pro[t].Enabled = true;                   //시간 정보를 다시 입력할 수 있게 만듬 
                tm_Parr[t].Stop();                          //타이머 종료
            }
            else {                                           //버튼이 눌리지 않았던 상태에서 버튼을 누른 경우       
                b_Pclick[t] = true;                          //눌렀다는 정보를 저장
                TB_Pro[t].Enabled = false;                   //시간 변경 불가
                BT_Pro[t].BackColor = Color.FromArgb(255, 255, 128); //버튼의 색을 노란색으로 만들어 줌
                time = Convert.ToInt32(TB_Pro[t].Text);               //입력한 시간을 변환해서
                if (time >= 1 && time <= 10) {                       //유효 시간은 (1 ~ 10초)
                    tm_Parr[t].Interval = time * 1000 - 400;               //타이머 시간 설정
                    tm_Parr[t].Start();                             //타이머 시작
                }       
            }
        }
        
        void Form1_Cus_BT(Object sender, EventArgs e) //소비자별 버튼 클릭시 처리하는 핸들러 함수
        {
            int t = Convert.ToInt32(((Button)sender).Tag.ToString()); //태그를 읽어와서 어떤 버튼이 눌렀는지 파별함
            int time = 0;    //타이머 시간 저장 임시 변수
            if (b_Cclick[t])    // 버튼이 눌렸었다면(소비자가 소비하고 있는 상태)
            {
                BT_Cus[t].BackColor = Color.Transparent;    //버튼의 색을 투명으로 변환
                b_Cclick[t] = false;                        //버튼 해제 정보를 저장
                TB_Cus[t].Enabled = true;                   //시간 정보를 다시 입력할 수 있게 만듬 
                tm_Carr[t].Stop();                          //타이머 종료
            }
            else
            {                                                         //버튼이 눌리지 않았던 상태에서 버튼을 누른 경우                  
                b_Cclick[t] = true;                                   //눌렀다는 정보를 저장
                TB_Cus[t].Enabled = false;                            //시간 변경 불가
                BT_Cus[t].BackColor = Color.FromArgb(255, 255, 128);  //버튼의 색을 노란색으로 만들어 줌                                                                       
                time = Convert.ToInt32(TB_Cus[t].Text);               //입력한 시간을 변환해서
                if (time >= 1 && time <= 10){                         //유효 시간은 (1 ~ 10초)                                                                        
                    tm_Carr[t].Interval = time * 1000 - 400;          //타이머 시간 설정
                    tm_Carr[t].Start();                               //타이머 시작
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //버퍼 인덱스 초기화
            this.idx_c = 0;
            this.idx_p = 0;
            for (int i = 0; i < 5; i++)
            {
                cnt_P_arr[i] = 0;   //생산자 트럭 이동 위치 저장 배열 초기화
                cnt_C_arr[i] = 0;   //소비자 트럭 이동 위치 저장 배열 초기화

                b_PTruck[i] = false;    //생산자 버튼 클릭 여부 저장 배열 초기화
                b_CTruck[i] = false;    //소비자 버튼 클릭 여부 저장 배열 초기화

                //생산자 트럭 이미지 배열 초기화(컨트롤 연결)
                truck_P[i].Visible = false;
                truck_P[i].SetBounds(i_xP, i_y, truck_P[i].Width, truck_P[i].Height);

                //소비자 트럭 이미지 배열 초기화(컨트롤 연결)
                truck_C[i].Visible = false;
                truck_C[i].SetBounds(i_xC, i_y, truck_C[i].Width, truck_C[i].Height);

                //각 텍스트 박스 출력 값 0 으로 초기화
                TB_Pro[i].Text = "0";
                TB_Cus[i].Text = "0";
                
                //텍스트박스 사용가능으로 설정
                TB_Pro[i].Enabled = true;                
                TB_Cus[i].Enabled = true;

                //버튼 색을 투명으로 초기화
                BT_Pro[i].BackColor = Color.Transparent;
                BT_Cus[i].BackColor = Color.Transparent;

                //생산자별 타이머 초기화
                tm_Parr[i].Interval = 1000;
                tm_Parr[i].Stop();

                //소비자별 타이머 초기화
                tm_Carr[i].Interval = 1000;
                tm_Carr[i].Stop();
            
                //클럭 여부 저장 배열 초기화
                b_Pclick[i] = false;
                b_Cclick[i] = false;
            }

            for (int i = 0; i < 16; i++)
            {
                //원형 버퍼 이미지 저장 배열 초기화(컨트롤 연결)
                pArr[i].Visible = false;
                enImg[i] = false;
            }
        }
    }
}
