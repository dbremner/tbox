// This is the main DLL file.
#include "stdafx.h"
#include "windows.h"
#include "Shlwapi.h"
#include "NativeBoost.h"
#include "FileSystem\file.h"
#include "Strings\strings.h"
#include <msclr\marshal_cppstd.h>

using namespace msclr::interop;

namespace NativeBoost 
{
	void Tools::ClearArray(IntPtr ptr)
	{
		delete[](void*)ptr.ToPointer();
	}

	template<typename F>
	inline bool searchInFile(const tchar* path, F op)
	{
		fs::File f(path);
		if(!f.IsOpened() )return false;
		tstring buf;
		buf.reserve(f.GetSize());
		while(f.ReadLine(buf)){
			if(op(buf.c_str()))return true;
		}
		return false;
	}

	inline bool Search(const tchar* path, const tchar* text, bool caseSensitive)
	{
		return caseSensitive ? 
			searchInFile(path, 
				[text](const tchar* line) -> bool {return wcsstr(line, text)!=nullptr;}) :
			searchInFile(path, 
				[text](const tchar* line) -> bool {return str::tstristr(line, text)!=nullptr;}); 

	}

	bool FileSystem::FileContains(String^ path, String^ text, bool caseSensitive)
	{
		return Search(
			marshal_as<tstring>(path).c_str(), 
			marshal_as<tstring>(text).c_str(), 
			caseSensitive);
	}

	void* ReadFileContentNative(const tchar* path)
	{
		fs::File f(path);
		if(!f.IsOpened() ) return 0;
		auto buf = f.ReadAll();
		auto ret = str::utf16_to_utf8(buf);
		delete[] buf;
		return ret;
	}

	IntPtr FileSystem::ReadFileContent(String^ path)
	{
		return IntPtr(ReadFileContentNative( marshal_as<tstring>(path).c_str()));
	}

}