#ifndef __ACTIONSCENE_SCENE_H__
#define __ACTIONSCENE_SCENE_H__

#include "cocos2d.h"

USING_NS_CC;

class ActionScene : public cocos2d::Layer
{
private:
	Sprite*		m_pSprite;
public:
	static cocos2d::Scene* createScene();

	virtual bool init();
	void CallBackFunc();
	void OnKeyPress(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event);
	void OnKeyUp(cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event);

	CREATE_FUNC(ActionScene);
};

#endif // __ACTIONSCENE_SCENE_H__