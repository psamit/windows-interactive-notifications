#include <node.h>
#include <v8.h>
#include <nan.h>

#include "InteractiveNotifications.h"
#include <malloc.h>
#include <windows.h>
#include <stdio.h>

typedef void (__stdcall *f_funci)();

using v8::Integer;
using v8::Local;
using v8::String;

NAN_METHOD(RegisterAppForNotificationSupport)
{
	Nan::HandleScope scope;

	if (info.Length() < 2 || !info[0]->IsString() || !info[1]->IsString())
	{
		Nan::ThrowTypeError("Invalid arguments, expected arguments are: shortcut name[string], app id [string]");
		return;
	}

	HRESULT hr = InteractiveNotifications::RegisterAppForNotificationSupport((PCWSTR)*info[0], (PCWSTR)*info[1]);

	if (hr != S_OK)
	{
		Nan::ThrowError(Nan::New<String>("RegisterAppForNotificationsSupport Failed").ToLocalChecked());
		return;
	}
}

NAN_METHOD(RegisterComServer)
{
	Nan::HandleScope scope;

	InteractiveNotifications::RegisterComServer();
}

NAN_METHOD(RegisterActivator)
{
	Nan::HandleScope scope;

	InteractiveNotifications::RegisterActivator();
}

NAN_METHOD(UnregisterActivator)
{
	Nan::HandleScope scope;

	InteractiveNotifications::UnregisterActivator();
}

NAN_MODULE_INIT(init) {
  // TODO: Make sure that this is necesarry..
  HRESULT hr = CoInitializeEx(NULL, COINITBASE_MULTITHREADED);

  Nan::SetMethod(target, "registerComServer", RegisterComServer);
  Nan::SetMethod(target, "registerAppForNotificationSupport", RegisterAppForNotificationSupport);
  Nan::SetMethod(target, "registerActivator", RegisterActivator);
  Nan::SetMethod(target, "unregisterActivator", UnregisterActivator);
}

NODE_MODULE(notifications_bindings, init)