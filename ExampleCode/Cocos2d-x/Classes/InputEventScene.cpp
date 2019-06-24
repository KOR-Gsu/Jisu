#include "InputEventScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* InputEventScene::createScene()
{
	auto scene = Scene::create();
	auto layer = InputEventScene::create();

	scene->addChild(layer);

	return scene;
}

bool InputEventScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

#if(CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
	auto mouse_listener = EventListenerMouse::create();
	
	mouse_listener->onMouseDown = CC_CALLBACK_1(InputEventScene::OnMouseDown1, this);
	mouse_listener->onMouseUp = CC_CALLBACK_1(InputEventScene::OnMouseUp1, this);
	mouse_listener->onMouseMove = CC_CALLBACK_1(InputEventScene::OnMouseMove1, this);
	mouse_listener->onMouseScroll = CC_CALLBACK_1(InputEventScene::OnMouseScroll1, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(mouse_listener, this);

	auto key_listener = EventListenerKeyboard::create();

	key_listener->onKeyPressed = CC_CALLBACK_2(InputEventScene::OnKeyPress, this);
	key_listener->onKeyReleased = CC_CALLBACK_2(InputEventScene::OnKeyUp, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(key_listener, this);
#elif(CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)

	auto listener = EventListenerTouchOneByOne::create();
	listener->onTouchBegan = CC_CALLBACK_1(InputEventScene::onTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_1(InputEventScene::onTouchMoved, this);
	listener->onTouchCancelled = CC_CALLBACK_1(InputEventScene::onTouchCancelled, this);
	listener->onTouchEnded = CC_CALLBACK_1(InputEventScene::onTouchEnded, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, this);

#endif

	return this;
}

#if(CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
void InputEventScene::OnKeyPress(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{
	switch (keyCode)
	{
		case EventKeyboard::KeyCode::KEY_UP_ARROW:
			log("key up");
		break;
	}
}

void InputEventScene::OnKeyUp(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{

}

void InputEventScene::OnMouseDown1(cocos2d::Event* event)
{
	EventMouse* e = (EventMouse*)event;
	log("%f , %f", e->getCursorX(), e->getCursorY());
}

void InputEventScene::OnMouseUp1(cocos2d::Event* event)
{

}

void InputEventScene::OnMouseMove1(cocos2d::Event* event)
{

}

void InputEventScene::OnMouseScroll1(cocos2d::Event* event)
{

}

#elif(CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)

void InputEventScene::OnTouchBegan(Touch* touch, Event* unused_event)
{
	EventMouse* e = (EventMouse*)event;
	log("%f , %f", e->getCursorX(), e->getCursorY());
}

void InputEventScene::OnTouchMoved(Touch* touch, Event* unused_event)
{

}

void InputEventScene::OnTouchCancelled(Touch* touch, Event* unused_event)
{

}

void InputEventScene::OnTouchEnded(Touch* touch, Event* unused_event)
{

}

#endif
