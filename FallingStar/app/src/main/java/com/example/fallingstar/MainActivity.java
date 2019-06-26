package com.example.fallingstar;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Point;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.Display;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;

import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends AppCompatActivity {

    Bitmap BG;                              // 배경
    Bitmap Player;                          // 플레이어
    Bitmap Star;                            // 별

    Point PlayerPos = new Point();          // 플레이어 위치
    Point TouchPos = new Point();           // 터치 위치
    Point MapSize = new Point();            // 맵 크기

    Rect PlayerRc = new Rect();

    ListQueue StarPos = new ListQueue();       // 별 위치

    MyView myView;

    Timer timer_generating = new Timer();
    Timer timer_falling = new Timer();
    Timer timer_restart = new Timer();

    boolean LeftPress = false, RightPress = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        myView= new MyView(this);
        setContentView(myView);

        Display display = ((WindowManager)getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay();
        display.getSize(MapSize);

        BG = BitmapFactory.decodeResource(getResources(), R.drawable.bg);

        Player = BitmapFactory.decodeResource(getResources(), R.drawable.player);
        Player = Bitmap.createScaledBitmap(Player, MapSize.x / 10, MapSize.y / 19, true);
        PlayerPos.x = MapSize.x * 1/ 2;
        PlayerPos.y = MapSize.y * 8/10;
        PlayerRc.left = PlayerPos.x;
        PlayerRc.top = PlayerPos.y;
        PlayerRc.right = PlayerPos.x + MapSize.x / 10;
        PlayerRc.bottom = PlayerPos.y + MapSize.y / 19;

        Star = BitmapFactory.decodeResource(getResources(), R.drawable.star);
        Star = Bitmap.createScaledBitmap(Star, MapSize.x / 15, MapSize.y / 20, true);

        timer_generating.schedule(new GeneratingTimer(), 0, 300);
        timer_falling.schedule(new FallingTimer(), 0, 100);
    }

    class RestartTimer extends  TimerTask{
        @Override
        public void run() {
            timer_generating.schedule(new GeneratingTimer(), 0, 300);
            timer_falling.schedule(new FallingTimer(), 0, 100);
        }
    }

    class GeneratingTimer extends  TimerTask{
        @Override
        public void run() {

            // 랜덤 별 위치
            int RandX = new Random().nextInt(MapSize.x - 1) + 1;

            // 별 위치 추가
            Point RandPos = new Point(RandX, 0);
            StarPos.enQueue(RandPos);
        }
    }

    class FallingTimer extends TimerTask {
        @Override
        public void run() {
            if (!StarPos.isEmpty()) {
                Node tmp = StarPos.Front;
                while (tmp != null) {

                    tmp.Pos.y += 30;
                    tmp.Rc.top += 30;
                    tmp.Rc.bottom += 30;

                    Point Center = new Point();
                    Center.x = tmp.Rc.centerX();
                    Center.y = tmp.Rc.centerY();
                    if(IsIntersect(Center, PlayerRc)) {
                        timer_falling.cancel();
                        timer_generating.cancel();
                        timer_restart.schedule(new RestartTimer(), 3000);
                    }

                    else if (tmp.Pos.y > MapSize.y)
                        StarPos.deQueue();

                    tmp = tmp.next;
                }
            }
            myView.postInvalidate();
        }
    }

    public boolean IsIntersect(Point Pt, Rect Rc)
    {
        if(Pt.x >= Rc.left && Pt.x <= Rc.right && Pt.y >= Rc.top && Pt.y <= Rc.bottom)
            return true;

        return false;
    }

    class MyView extends View {
        MyView(Context context) {
            super(context);
        }

        @Override
        public void onDraw(Canvas canvas) {
            Paint p1 = new Paint();
            canvas.drawBitmap(BG, 0, 0, p1);
            canvas.drawBitmap(Player, PlayerPos.x, PlayerPos.y, p1);
            if (!StarPos.isEmpty()) {
                Node tmp = StarPos.Front;
                while (tmp != null) {
                    canvas.drawBitmap(Star, tmp.Pos.x, tmp.Pos.y, p1);

                    tmp = tmp.next;
                }
            }
        }

        @Override
        public boolean onTouchEvent(MotionEvent event) {

            int action = event.getAction();

            switch (action) {
                case MotionEvent.ACTION_DOWN:
                    if (!LeftPress && !RightPress) {
                        TouchPos.x = (int) event.getX();
                        TouchPos.y = (int) event.getY();

                        if ((TouchPos.x >= 0) && (TouchPos.x < MapSize.x / 2))
                            LeftPress = true;
                        if ((TouchPos.x > MapSize.x / 2) && (TouchPos.x <= MapSize.x))
                            RightPress = true;
                    }
                    break;
                case MotionEvent.ACTION_MOVE:
                    if (!LeftPress && !RightPress) {
                        TouchPos.x = (int) event.getX();
                        TouchPos.y = (int) event.getY();

                        if ((TouchPos.x >= 0) && (TouchPos.x < MapSize.x / 2))
                            LeftPress = true;
                        if ((TouchPos.x > MapSize.x / 2) && (TouchPos.x <= MapSize.x))
                            RightPress = true;
                    }
                    break;
                case MotionEvent.ACTION_UP:
                    LeftPress = RightPress = false;
                    break;
            }

            if (LeftPress || RightPress) {
                if (LeftPress) {
                    PlayerPos.x -= 10;
                    PlayerRc.left -= 10;
                    PlayerRc.right -= 10;
                }
                if (RightPress) {
                    PlayerPos.x += 10;
                    PlayerRc.left += 10;
                    PlayerRc.right += 10;
                }
            }
            invalidate();
            return true;
        }
    }

    class Node{
        Point Pos;
        Rect Rc;
        Node next;
    }
    class ListQueue {
        private Node Front;
        private Node Rear;

        public ListQueue(){
            this.Front = null;
            this.Rear = null;
        }

        public boolean isEmpty(){
            if(this.Front == null)
                return true;
            else
                return false;
        }

        public void enQueue(Point Data){
            Node newNode = new Node();
            newNode.Pos = Data;
            newNode.Rc = new Rect();
            newNode.Rc.contains(Data.x, Data.y, Data.x + MapSize.x / 15, Data.y +  MapSize.y / 20);

            newNode.next = null;

           if(isEmpty())
           {
               this.Front = newNode;
               this.Rear = newNode;
           }
           else
           {
               this.Rear.next = newNode;
               this.Rear = newNode;
           }
        }

        public void deQueue(){
           if(!isEmpty())
           {
                this.Front = this.Front.next;
           }
        }
    }
}
