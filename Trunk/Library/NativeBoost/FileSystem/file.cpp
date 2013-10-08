#include "stdafx.h"
#include "file.h"
#include <iostream>
#include <string>

namespace fs
{
	File::File(const tchar* path):f(nullptr),path(path)
	{
		if(_wfopen_s(&f, path, L"rtS, ccs=UTF-8") != 0 )
		{
			f=nullptr;
		}
	}
	
	File::~File()
	{
		if(f!=nullptr)fclose(f);
	}
	
	bool File::IsOpened()const
	{
		return f!=nullptr;
	};
	
	unsigned int File::GetSize()const
	{
		struct _stat fileinfo;
		_wstat(path, &fileinfo);
		return fileinfo.st_size;
	}

	void File::ReadAll(tstring& buf)
	{
		auto size = GetSize();
		buf.resize(size+1);
		size_t wchars_read = fread(&(buf.front()), sizeof(tchar), buf.length(), f);
		buf.resize(wchars_read);
		//buf.shrink_to_fit();
	}

	bool File::ReadLine(tstring& buf)
	{
		return fgetws(&(buf.front()), (int)buf.capacity(), f) !=nullptr;
	}

	tchar* File::ReadAll()
	{
		auto size = GetSize();
		tchar* buf = new tchar[size+1];
		size_t wchars_read = fread(buf, sizeof(tchar), size, f);
		buf[wchars_read] = 0;
		return buf;
	}

}