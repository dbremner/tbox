#pragma once
#include "..\Common.h"

namespace fs{

class File
{
public:
	File(const tchar* path);
	~File();
	bool IsOpened()const;
	unsigned int GetSize()const;
	void ReadAll(tstring& buf);
	tchar* ReadAll();

private:
	FILE*f;
	const tchar*path;
};

}