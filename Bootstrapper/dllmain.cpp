// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "thread"
#include <iostream>
#include "Windows.h"

#include "metahost.h"
#include "mscoree.h"

#pragma comment(lib, "mscoree.lib")

typedef HRESULT(__stdcall* GetInterface)(REFCLSID rclsid, REFIID riid, LPVOID* ppUnk);

int bootstrap() {
    AllocConsole();
    freopen_s((FILE**) stdout,"CONOUT$", "w", stdout);
    printf("Entered bootstrapper \n");

    ICLRMetaHost* metaHost = NULL;
    auto res = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&metaHost);

    if (res != S_OK) {
        printf("Failed to create instance");
        return 0;
    }

    ICLRRuntimeInfo* runtimeInfo = NULL;
    auto res1 = metaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&runtimeInfo);

    if (res != S_OK) {
        printf("Failed to get runtime");
        return 0;
    }

    ICLRRuntimeHost* runtimeHost = NULL;
    auto res1_5 = runtimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&runtimeHost);

    if (res1_5 != S_OK) {
        printf("Failed to get interface");
        return 0;
    }

    runtimeHost->Start();
    DWORD retval;

    auto path = L"D:\\OldmanSEB.dll"; // You can potentially use system relative paths (through std::filesystem) if you have
    // ASCII abiding characters only in the file name (make sure the compiled .NET framework dll is reflected in the path)
    auto res2 = runtimeHost->ExecuteInDefaultAppDomain(path, L"OldmanSEB.Patcher", L"Initialize", L"", &retval);
    if (res2 != S_OK) {
        printf("Failed to execute function");
        return 0;
    }

    printf("Succeded: %d \n", retval);
    return 1;
}

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    using namespace std::chrono_literals;
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH: {
        std::thread th(bootstrap);
        th.detach();
    }
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

