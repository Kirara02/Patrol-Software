#pragma once

#include <stddef.h>
#include <stdint.h>

#include "ecl.h"
#include "system.h"

ECL_PUBLIC size_t ECLStream_write_stackSize(void);

ECL_PUBLIC void *ECLStream_write_stackInit(void *stack,
                                           int fdIn, int fdOut,
                                           const uint8_t *key, size_t keySize,
                                           size_t *outSize);

ECL_PUBLIC enum syscall_e ECLStream_write(void *stack, struct system_s *system);
