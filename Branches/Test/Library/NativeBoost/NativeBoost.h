// NativeBoost.h

#pragma once

using namespace System;

namespace NativeBoost {

	public ref class Tools abstract sealed
	{
	public:
		static void ClearArray(IntPtr ptr);
	};

	public ref class FileSystem abstract sealed
	{
	public:
		static bool FileContains(String^ path, String^ text, bool caseSensitive);
		static IntPtr ReadFileContent(String^ path);
	};
}
