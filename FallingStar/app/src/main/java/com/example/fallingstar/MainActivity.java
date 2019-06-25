package com.example.fallingstar;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Point;
import android.os.Bundle;
import android.view.Display;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;

import java.util.LinkedList;
import java.util.Queue;
import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends AppCompatActivity {

    Bitmap BG;                       // 배경
    Bitmap Player;                   // 플레이어
    Bitmap Star;                     // 별

    Point PlayerPos = new Point();   // 플레이어 위치
    Point TouchPos = new Point();    // 터치 위치
    Point MapSize = new Point();     // 맵 크기

    Queue<Point> StarPos = new LinkedList<Point>();            // 별 위치

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        MyView myView = new MyView(this);
        setContentView(myView);

        Display display = ((WindowManager)getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay();
        display.getSize(MapSize);

        BG = BitmapFactory.decodeResource(getResources(), R.drawable.background);

        Player = BitmapFactory.decodeResource(getResources(), R.drawable.player);
        Player = Bitmap.createScaledBitmap(Player, MapSize.x / 8, MapSize.y / 11, true);
        PlayerPos.x = MapSize.x /2;
        PlayerPos.y = MapSize.y - 10;

        Star = BitmapFactory.decodeResource(getResources(), R.drawable.star);

        Timer timer = new Timer();
        timer.schedule(new CustomTimer(), 0, 500);
    }

    class CustomTimer extends TimerTask {
        @Override
        public void run()
        {
            // 랜덤 별 위치
            int RandX = new Random().nextInt(MapSize.x - 1) + 1;

            // 별 위치 추가
            Point RandPos = new Point(RandX, 0);
            StarPos.offer(RandPos);

            while(StarPos.peek() != null) {

                StarPos.peek().y += 10;

                if(StarPos.peek().y > MapSize.y)
                    StarPos.poll();
            }
        }
    }

    class MyView extends View {
        MyView(Context context) {
            super(context);
        }

        @Override
        public void OnDraw(Canvas canvas) {
            Paint p1 = new Paint();
            canvas.drawBitmap(BG, 0, 0, p1);
            canvas.drawBitmap(Player, PlayerPos.x, PlayerPos.y, p1);

            while (StarPos.peek() != null)
                canvas.drawBitmap(Star, StarPos.peek().x, StarPos.peek().y, p1);
        }

        @Override
        public boolean OnTouchEvent(MotionEvent event){
            if(event.getAction() == MotionEvent.ACTION_DOWN || event.getAction() == MotionEvent.ACTION_MOVE)
            {
                TouchPos.x = (int)event.getX();
                TouchPos.y = (int)event.getY();
            }

            if((TouchPos.x >= 0) && (TouchPos.y < MapSize.x / 2))
                PlayerPos.x -= 20;

            if((TouchPos.x > MapSize.x / 2) && (TouchPos.x <= MapSize.x))
                PlayerPos.x += 20;

            invalidate();
            return true;
        }
    }

    public class ArrayQueue{
        private int front = -1;
        private int rear = -1;
        private int MaxSize = 0;
        private Object[] queueArray;

        ArrayQueue(int Size){
            this.MaxSize = Size;
            this.queueArray = new Object[Size];
        }

        public void enQueue(Object element){
            if(!isFull()){
                queueArray[++rear] = element;
            }
        }

        public Object deQueue(){
            Object tmp = null;
            if(!isEmpty())
            {
                ++front;
                tmp = queueArray[front];

                queueArray[front] = null;

                if(isEmpty()){
                    front = -1;
                    rear = -1;
                }

            }
            return tmp;
        }

        public  Boolean isFull(){
            return rear == MaxSize - 1 ? true : false;
        }

        public Boolean isEmpty(){
            return front == rear ? true : false;
        }
    }
}
