; ModuleID = 'clangvla.c'
source_filename = "clangvla.c"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-i128:128-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.44.35209"

; Function Attrs: noinline nounwind optnone uwtable
define dso_local i32 @main() #0 !dbg !9 {
  %1 = alloca i32, align 4
  %2 = alloca i32, align 4
  %3 = alloca ptr, align 8
  %4 = alloca i64, align 8
  store i32 0, ptr %1, align 4
    #dbg_declare(ptr %2, !14, !DIExpression(), !15)
  %5 = load i32, ptr %2, align 4, !dbg !16
  %6 = zext i32 %5 to i64, !dbg !16
  %7 = call ptr @llvm.stacksave.p0(), !dbg !16
  store ptr %7, ptr %3, align 8, !dbg !16
  %8 = alloca i32, i64 %6, align 16, !dbg !16
  store i64 %6, ptr %4, align 8, !dbg !16
    #dbg_declare(ptr %4, !17, !DIExpression(), !19)
    #dbg_declare(ptr %8, !20, !DIExpression(), !16)
  %9 = getelementptr inbounds i32, ptr %8, i64 0, !dbg !24
  store i32 42, ptr %9, align 16, !dbg !24
  store i32 0, ptr %1, align 4, !dbg !25
  %10 = load ptr, ptr %3, align 8, !dbg !26
  call void @llvm.stackrestore.p0(ptr %10), !dbg !26
  %11 = load i32, ptr %1, align 4, !dbg !26
  ret i32 %11, !dbg !26
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare ptr @llvm.stacksave.p0() #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.stackrestore.p0(ptr) #1

attributes #0 = { noinline nounwind optnone uwtable "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cmov,+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nocallback nofree nosync nounwind willreturn }

!llvm.dbg.cu = !{!0}
!llvm.module.flags = !{!2, !3, !4, !5, !6, !7}
!llvm.ident = !{!8}

!0 = distinct !DICompileUnit(language: DW_LANG_C99, file: !1, producer: "clang version 20.1.7 (https://github.com/wareya/llvm-custom-builds fb64f1750b46b9b3116ef126a3b0c61566cbae96)", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, splitDebugInlining: false, nameTableKind: None)
!1 = !DIFile(filename: "clangvla.c", directory: "D:\\Git\\GitHub\\Steadsoft\\Syscode\\TestSource", checksumkind: CSK_MD5, checksum: "48951eb8dd775898abfc7d8a7610e7be")
!2 = !{i32 2, !"CodeView", i32 1}
!3 = !{i32 2, !"Debug Info Version", i32 3}
!4 = !{i32 1, !"wchar_size", i32 2}
!5 = !{i32 8, !"PIC Level", i32 2}
!6 = !{i32 7, !"uwtable", i32 2}
!7 = !{i32 1, !"MaxTLSAlign", i32 65536}
!8 = !{!"clang version 20.1.7 (https://github.com/wareya/llvm-custom-builds fb64f1750b46b9b3116ef126a3b0c61566cbae96)"}
!9 = distinct !DISubprogram(name: "main", scope: !1, file: !1, line: 3, type: !10, scopeLine: 3, spFlags: DISPFlagDefinition, unit: !0, retainedNodes: !13)
!10 = !DISubroutineType(types: !11)
!11 = !{!12}
!12 = !DIBasicType(name: "int", size: 32, encoding: DW_ATE_signed)
!13 = !{}
!14 = !DILocalVariable(name: "n", scope: !9, file: !1, line: 4, type: !12)
!15 = !DILocation(line: 4, scope: !9)
!16 = !DILocation(line: 5, scope: !9)
!17 = !DILocalVariable(name: "__vla_expr0", scope: !9, type: !18, flags: DIFlagArtificial)
!18 = !DIBasicType(name: "unsigned long long", size: 64, encoding: DW_ATE_unsigned)
!19 = !DILocation(line: 0, scope: !9)
!20 = !DILocalVariable(name: "arr", scope: !9, file: !1, line: 5, type: !21)
!21 = !DICompositeType(tag: DW_TAG_array_type, baseType: !12, elements: !22)
!22 = !{!23}
!23 = !DISubrange(count: !17)
!24 = !DILocation(line: 6, scope: !9)
!25 = !DILocation(line: 7, scope: !9)
!26 = !DILocation(line: 8, scope: !9)
