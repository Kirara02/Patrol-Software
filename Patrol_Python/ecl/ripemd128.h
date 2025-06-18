#pragma once

#include <stddef.h>
#include <stdint.h>

#include "ecl.h"

ECL_PUBLIC void ripemd128_calc(uint8_t out[static 16], const uint8_t *input, size_t inputSize);
