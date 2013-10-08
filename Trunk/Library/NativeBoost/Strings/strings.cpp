#include "stdafx.h"
#include "strings.h"
#include <codecvt>
namespace str
{
	char* utf16_to_utf8 (const wchar_t* str)
	{
		if (str == nullptr) return nullptr;

		int utf16len = (int)wcslen(str);
		int utf8len = WideCharToMultiByte(CP_UTF8, 0, str, utf16len, 0, 0, 0, 0 );

		char* buffer = new char[utf8len+1];
		buffer[utf8len]=0;

		WideCharToMultiByte(CP_UTF8, 0, str, utf16len, buffer, utf8len, 0, 0 );

		return buffer;
	}

	inline tchar toUpper(tchar ch)
	{
		if(ch >= L'à' && ch <= L'ÿ' )return ch + L'À'-L'à';
		if(ch == L'¸' )return L'¨';
		return toupper(ch);
	}
	
	tchar* tstristr(const tchar *text, const tchar *substring)
	{
		if (!text || !substring || !*text || !*substring)
			return nullptr;
		auto test(toUpper(*substring));
		while (*text)
		{
			if (toUpper(*text)==test)
			{
				for(register auto i=text+1, j=substring+1;;++i,++j)
				{
					auto cj = *j;
					if(!cj )return (tchar*)(text);
					auto ci = *i;
					if(!ci )break;
					if (toUpper(ci)!=toUpper(cj))break;
				}
			}
			++text;
		}
		return nullptr;
	} 
}