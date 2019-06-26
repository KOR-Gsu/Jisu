package com.example.fallingstarsurface;

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
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.WindowManager;

import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends AppCompatActivity {

    Point MapSize = new Point();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(new MySurfaceView(this));
    }

    public class MySurfaceView extends SurfaceView implements SurfaceHolder.Callback {
        Context mContext;
        StarThread mStarThread;
        RenderingThread mRenderingThread;
        SurfaceHolder mHolder;

        Bitmap Player;
        Bitmap Star;
        Bitmap BG;

        Point PlayerPos = new Point();
        Point TouchPos = new Point();

        ListQueue StarPos = new ListQueue();

        Rect PlayerRc = new Rect();

        boolean LeftPress = false, RightPress = false;

        public MySurfaceView(Context context) {
            super(context);
            mContext = context;
            mHolder = getHolder();
            mHolder.addCallback(this);
            mStarThread = new StarThread();
            mRenderingThread = new RenderingThread();

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
        }

        @Override
        public void surfaceCreated(SurfaceHolder surfaceHolder) {
            mStarThread.start();
            mRenderingThread.start();
        }

        @Override
        public void surfaceChanged(SurfaceHolder surfaceHolder, int format, int width, int height){

        }

        @Override
        public void surfaceDestroyed(SurfaceHolder surfaceHolder){
            try{
                mRenderingThread.join();
            }catch (InterruptedException e){
                e.printStackTrace();
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

        class StarThread extends Thread {

            Timer timer_generating = new Timer();
            Timer timer_falling = new Timer();
            Timer timer_restart = new Timer();

            public void run(){
                timer_generating.schedule(new GeneratingTimer(), 0, 300);
                timer_falling.schedule(new FallingTimer(), 0, 100);
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
                }

                class RestartTimer extends TimerTask {
                    @Override
                    public void run() {
                        timer_generating.schedule(new GeneratingTimer(), 0, 300);
                        timer_falling.schedule(new FallingTimer(), 0, 100);
                    }
                }

                public boolean IsIntersect(Point Pt, Rect Rc)
                {
                    if(Pt.x >= Rc.left && Pt.x <= Rc.right && Pt.y >= Rc.top && Pt.y <= Rc.bottom)
                        return true;

                    return false;
                }
            }
        }

        class RenderingThread extends Thread {

            public RenderingThread(){
                Player = BitmapFactory.decodeResource(getResources(), R.drawable.player);
                Star = BitmapFactory.decodeResource(getResources(), R.drawable.star);
                BG = BitmapFactory.decodeResource(getResources(), R.drawable.bg);
            }

            public void run() {
                Canvas canvas = null;
                while (true) {
                    canvas = mHolder.lockCanvas();
                    try {
                        synchronized (mHolder) {
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
                    } finally {
                        if (canvas != null) {
                            mHolder.unlockCanvasAndPost(canvas);
                        }
                    }
                }
            }
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
