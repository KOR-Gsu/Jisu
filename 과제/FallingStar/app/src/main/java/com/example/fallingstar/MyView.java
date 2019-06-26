package com.example.fallingstar;

import android.graphics.Canvas;
import android.view.MotionEvent;

interface MyView {
    void OnDraw(Canvas canvas);

    boolean OnTouchEvent(MotionEvent event);
}
