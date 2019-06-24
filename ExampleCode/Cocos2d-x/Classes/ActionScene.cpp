#include "ActionScene.h"
#include "SimpleAudioEngine.h"

USING_NS_CC;

Scene* ActionScene::createScene()
{
	auto scene = Scene::create();
	auto layer = ActionScene::create();
	scene->addChild(layer);

	return scene;
}

bool ActionScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	m_pSprite = Sprite::create("grossiniaa.png");
	m_pSprite->setPosition(Vec2(500, 100));
	addChild(m_pSprite, 3);

	//this->schedule

	MoveBy* pAction = MoveBy::create(1, Vec2(200, 0));
	//m_pSprite->runAction(pAction);
	
	Sequence* pActionSe = Sequence::create(pAction , pAction->reverse(), NULL);
	RepeatForever* forever = RepeatForever::create(pActionSe);
	m_pSprite->runAction(forever);
	
	//MoveTo* pAction = MoveTo::create(2.0f, Vec2(100, 100));
	//m_pSprite->runAction(pAction);

	//MoveBy* pAction = MoveBy::create(1.0f, Vec2(100, 100));
	//pSprite->runAction(pAction);

	//JumpTo* pAction = JumpTo::create(2.0f, Vec2(300, 100) , 200 , 1);
	//m_pSprite->runAction(pAction);

	//JumpBy* pAction = JumpBy::create(1.0f, Vec2(100, 100) , 200, 1);
	//m_pSprite->runAction(pAction);

	/*
	ccBezierConfig bezier;
	bezier.controlPoint_1 = Vec2(300, 500);
	bezier.controlPoint_2 = Vec2(300, 200);
	bezier.endPosition = Vec2(500, 0);
	BezierTo* pAction = BezierTo::create(3.0f, bezier);
	m_pSprite->runAction(RepeatForever::create(pAction));
	*/

	//MoveBy* pJAction = MoveBy::create(2.0f, Vec2(300, 100));
	//ScaleTo* pMAction = ScaleTo::create(2.0f, 2);

	//Sequence *action = Sequence::create(pJAction, pMAction, NULL);
	//Spawn *action = Spawn::create(pJAction, pMAction, NULL);

	//DelayTime* action_time = DelayTime::create(1.0f);
	//Sequence *action = Sequence::create(pJAction , action_time , pJAction->reverse(), NULL);
	//pSprite->runAction(action);

	//Repeat
	//RepeatForever* actionRe = RepeatForever::create(action);
	//m_pSprite->runAction(actionRe);
	
	//Place* move = Place::create(Vec2(500, 768));
	//Sequence *action = Sequence::create(pAction, move, NULL);

	//RepeatForever* actionRe = RepeatForever::create(action);
	//m_pSprite->runAction(actionRe);
	
	CallFunc* pCallBackAction = CallFunc::create(CC_CALLBACK_0(ActionScene::CallBackFunc, this));
	Sequence *action = Sequence::create(pAction, pCallBackAction, NULL);

	// MoveTo , MoveBy //�̵�
	// JumpTo , JumpBy //����
	// BezierTo , BezierBy //������ �
	// Place // �����̵�
	// ScaleTo ScaleBy // Ȯ��
	// RotateBy RotateTo // ȸ��
	// Show Hide // ����
	// ToggleVisibility // ���� ���� �ݴ��
	// Blink //�����̱�
	// FadeIn , FadeOut // ���̵� �� ���̵� �ƿ�
	// FadeTo
	// FlipX , FlipY
	// RemoveSelf // �ڱ��ڽ� ���ֱ�
	// TintBy , TintTo // �÷�

	auto key_listener = EventListenerKeyboard::create();
	key_listener->onKeyPressed = CC_CALLBACK_2(ActionScene::OnKeyPress, this);
	key_listener->onKeyReleased = CC_CALLBACK_2(ActionScene::OnKeyUp, this);
	_eventDispatcher->addEventListenerWithSceneGraphPriority(key_listener, this);

	return true;
}

void ActionScene::CallBackFunc()
{

}

void ActionScene::OnKeyPress(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{
	switch (keyCode)
	{
	case EventKeyboard::KeyCode::KEY_UP_ARROW:
		this->_scheduler->setTimeScale(3.0f);
		break;
	case EventKeyboard::KeyCode::KEY_DOWN_ARROW:
		this->_scheduler->setTimeScale(1.0f);
		break;
	}

}

void ActionScene::OnKeyUp(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event)
{

}

