#pragma once

#ifndef ECL_PUBLIC
#if defined(_WIN32)
#define ECL_PUBLIC __declspec(dllimport)
#else
#define ECL_PUBLIC
#endif
#endif
