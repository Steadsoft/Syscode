; ModuleID = 'clangll.c'
source_filename = "clangll.c"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-i128:128-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.44.35209"

%struct.Bits = type { [64 x i8] }

@bits = dso_local global %struct.Bits zeroinitializer, align 1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local void @reboot() #0 {
  %1 = load i8, ptr getelementptr inbounds ([64 x i8], ptr @bits, i64 0, i64 5), align 1
  %2 = zext i8 %1 to i32
  %3 = or i32 %2, 8
  %4 = trunc i32 %3 to i8
  store i8 %4, ptr getelementptr inbounds ([64 x i8], ptr @bits, i64 0, i64 5), align 1
  %5 = load i8, ptr getelementptr inbounds ([64 x i8], ptr @bits, i64 0, i64 5), align 1
  %6 = zext i8 %5 to i32
  %7 = and i32 %6, -9
  %8 = trunc i32 %7 to i8
  store i8 %8, ptr getelementptr inbounds ([64 x i8], ptr @bits, i64 0, i64 5), align 1
  ret void
}

attributes #0 = { noinline nounwind optnone uwtable "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cmov,+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 8, !"PIC Level", i32 2}
!2 = !{i32 7, !"uwtable", i32 2}
!3 = !{i32 1, !"MaxTLSAlign", i32 65536}
!4 = !{!"clang version 20.1.7 (https://github.com/wareya/llvm-custom-builds fb64f1750b46b9b3116ef126a3b0c61566cbae96)"}
