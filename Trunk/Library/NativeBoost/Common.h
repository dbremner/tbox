#pragma once
#include <string>
#include "boost\algorithm\string.hpp"
#include <windows.h>
#include <iostream>
#include <fstream>

#define _T(x) L(x)

typedef wchar_t tchar; 
typedef std::wstring tstring; 
typedef std::wifstream tifstream;