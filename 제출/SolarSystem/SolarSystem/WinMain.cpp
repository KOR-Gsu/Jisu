#pragma once
#include"SolarSystem.h"

SolarSystem* g_SolarSystem;

LRESULT WINAPI MsgProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_DESTROY:
		g_SolarSystem->CleanUp();
		delete g_SolarSystem;
		PostQuitMessage(0);
		return 0;
	}
	return DefWindowProc(hWnd, msg, wParam, lParam);
}

INT WINAPI WinMain(HINSTANCE hInst, HINSTANCE, LPSTR, INT)
{
	WNDCLASSEX wc = { sizeof(WNDCLASSEX), CS_CLASSDC, MsgProc,
		0L, 0L, GetModuleHandle(NULL), NULL, NULL, NULL, NULL,
		L"D3D Hierarchy", NULL };

	RegisterClassEx(&wc);

	HWND hWnd = CreateWindow(L"D3D Hierarchy", L"D3D Hierarchy", WS_OVERLAPPEDWINDOW,
		100, 100, 1024, 768, GetDesktopWindow(), NULL, wc.hInstance, NULL);

	g_SolarSystem = new SolarSystem;

	if (SUCCEEDED(g_SolarSystem->Init(hWnd)))
	{
		ShowWindow(hWnd, SW_SHOWDEFAULT);
		UpdateWindow(hWnd);

		MSG msg;
		ZeroMemory(&msg, sizeof(msg));

		while (msg.message != WM_QUIT)
		{
			if (PeekMessage(&msg, NULL, 0U, 0U, PM_REMOVE))
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
			else
			{
				g_SolarSystem->Update();
				g_SolarSystem->Render();
			}
		}
	}

	UnregisterClass(L"D3D Hierarchy", wc.hInstance);
	return 0;
}