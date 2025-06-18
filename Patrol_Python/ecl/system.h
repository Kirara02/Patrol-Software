#pragma once

#include <stddef.h>
#include <stdint.h>
#include <stdio.h>
#include <unistd.h>
#include <sys/types.h>

enum syscall_e {
    SYSCALL_EXIT = 1,
    SYSCALL_YIELD = 2,
    // https://man7.org/linux/man-pages/man3/malloc.3.html
    SYSCALL_MEM_ALLOC = 3,
    SYSCALL_MEM_REALLOC = 4,
    SYSCALL_MEM_FREE = 5,
    // https://man7.org/linux/man-pages/man2/read.2.html
    SYSCALL_IO_READ_REL = 6,
    // https://man7.org/linux/man-pages/man2/pread.2.html
    SYSCALL_IO_READ_ABS = 7,
    // https://man7.org/linux/man-pages/man2/write.2.html
    SYSCALL_IO_WRITE_REL = 8,
    // https://man7.org/linux/man-pages/man2/pread.2.html
    SYSCALL_IO_WRITE_ABS = 9,
    // https://man7.org/linux/man-pages/man3/fflush.3.html + https://man7.org/linux/man-pages/man2/fsync.2.html
    SYSCALL_IO_FLUSH = 10,
};

struct system_s {
    struct {
        void *ptr;
        size_t size;
        void **ret;
    } mem;

    struct {
        int fd;

        union {
            void *mBuf;
            const void *cBuf;
        };

        size_t count;
        off_t offset;
        ssize_t *ret;
    } io;
};
