#include "TouchAndEventScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* TouchAndEventScene::createScene()
{
	auto scene = Scene::create();
	auto layer = TouchAndEventScene::create();
	scene->addChild(layer);

	return scene;
}

bool TouchAndEventScene::init()
{
	if (!Layer::init())
	{
		return false;
	}
	

#if	(CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
	auto listener = EventListenerMouse::create();
	listener->onMouseDown = CC_CALLBACK_1(TouchAndEventScene::OnMouseDown1, this);
	listener->onMouseUp = CC_CALLBACK_1(TouchAndEventScene::OnMouseUp1, this);
	listener->onMouseMove = CC_CALLBACK_1(TouchAndEventScene::OnMouseMove1, this);
	listener->onMouseScroll = CC_CALLBACK_1(TouchAndEventScene::OnMouseScroll1, this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, this);

	auto key_listener = EventListenerKeyboard::create();
	key_listener->onKeyPressed = CC_CALLBACK_2(TouchAndEventScene::OnKeyPress, this);
	key_listener->onKeyReleased = CC_CALLBACK_2(TouchAndEventScene::OnKeyUp, this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(key_listener, this);
#elif (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
	auto listener = EventListenerTouchOneByOne::create();
	listener->onTouchBegan = CC_CALLBACK_2(TouchAndEventScene::onTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_2(TouchAndEventScene::onTouchMoved, this);
	listener->onTouchCancelled = CC_CALLBACK_2(TouchAndEventScene::onTouchCancelled, this);
	listener->onTouchEnded = CC_CALLBACK_2(TouchAndEventScene::onTouchEnded, this);
#endif

	return true;
}

#if	(CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
void TouchAndEventScene::OnKeyPress(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{
	switch (keyCode)
	{
		case EventKeyboard::KeyCode::KEY_UP_ARROW:
			log("key up");
		break;
		case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
			log("key left");
		break;
	}

}

void TouchAndEventScene::OnKeyUp(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{

}

void TouchAndEventScene::OnMouseDown1(cocos2d::Event* event)
{
	EventMouse* e = (EventMouse*)event;
	log("%f , %f" , e->getCursorX() , e->getCursorY());
}

void TouchAndEventScene::OnMouseUp1(cocos2d::Event* event)
{

}

void TouchAndEventScene::OnMouseMove1(cocos2d::Event* event)
{

}

void TouchAndEventScene::OnMouseScroll1(cocos2d::Event* event)
{

}



#elif (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
bool TouchAndEventScene::onTouchBegan(Touch* touch, Event* unused_event)
{
	Point pos = touch->getLocation();
	log("%d , %d", pos.x, pos.y);
	return true;

}

void TouchAndEventScene::onTouchMoved(Touch* touch, Event* unused_event) 
{

}

void TouchAndEventScene::onTouchCancelled(Touch* touch, Event* unused_event) 
{

}

void TouchAndEventScene::onTouchEnded(Touch* touch, Event* unused_event) 
{

}
#endif

