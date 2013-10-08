#pragma once
#include "..\Common.h"

namespace str
{
	char* utf16_to_utf8 (const wchar_t* str);
	tchar* tstristr(const tchar *text, const tchar *substring);
}