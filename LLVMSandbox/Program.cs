using LLVMSharp;
using LLVMSharp.Interop;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace LLVMSandbox
{
    internal class Program
    {
        static unsafe void Main(string[] unused)
        {
            using var module = LLVMModuleRef.CreateWithName("test_call");
            var builder = module.Context.CreateBuilder();
                          
            LLVMTypeRef[] param_types = { LLVM.PointerType(LLVMTypeRef.Int16, 0) };
            LLVMTypeRef printf_type = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, param_types, true);
            // not adding a basic block etc, to this function, causes it to become just a 'declare' rather than a 'define'
            LLVMValueRef printf_func = module.AddFunction("printf", printf_type);

            //var blocky = printf_func.AppendBasicBlock(string.Empty);

            //builder.PositionAtEnd(blocky);

            using var format = new MarshaledString("Value: %d\n");
            using var fmt = new MarshaledString("fmt");


            

            int op1 = 3;
            int op2 = 4;

            var returnType = LLVMTypeRef.Int16;
            var name = "add";
            var parameterTypes = new LLVMTypeRef[] { LLVMTypeRef.Int16, LLVM.PointerType(LLVMTypeRef.Int16,0) };

            var type = LLVMTypeRef.CreateFunction(returnType, parameterTypes);
            var func = module.AddFunction(name, type);
            var block = func.AppendBasicBlock(string.Empty);
            builder.PositionAtEnd(block);

            LLVM.BuildGlobalStringPtr(builder, format, fmt);

            var p1 = func.GetParam(0);
            var p2 = func.GetParam(1);
            var zex = builder.BuildPtrToInt(p2, LLVMTypeRef.Int16); // builder.BuildZExt(p2, LLVMTypeRef.Int16);
            var add = builder.BuildAdd(p1, zex);
            var sub = builder.BuildMul(add, zex);
            var ret = builder.BuildRet(sub);

            /*
             * The above generates:
             *  
             *  
                define i16 @add(i16 %0, ptr %1) {
                  %3 = ptrtoint ptr %1 to i16
                  %4 = add i16 %0, %3
                  %5 = mul i16 %4, %3
                  ret i16 %5
                }              
             * 
             * 
             */


            //(var addType, var addDef) = module.AddFunction(
            //    LLVMTypeRef.Int32, "add", [LLVMTypeRef.Int32, LLVMTypeRef.Int32], (f, b) =>
            //    {
            //        var p1 = f.GetParam(0);
            //        var p2 = f.GetParam(1);
            //        var add = b.BuildAdd(p1, p2);
            //        var ret = b.BuildRet(add);
            //    });


            //(_, var entryDef) = module.AddFunction(
            //    LLVMTypeRef.Int32, "entry", [LLVMTypeRef.Int32, LLVMTypeRef.Int32], (f, b) =>
            //    {
            //        var call = b.BuildCall2(addType, addDef, f.GetParams(), "");
            //        var ret = b.BuildRet(call);
            //    });
            //module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);

            //_ = LLVM.InitializeNativeTarget();
            //_ = LLVM.InitializeNativeAsmParser();
            //_ = LLVM.InitializeNativeAsmPrinter();

            //var engine = module.CreateMCJITCompiler();
            //var func = engine.GetPointerToGlobal<Int32Int32Int32Delegate>(entryDef);
            //var result = op1 + op2;

            module.Dump();

            module.WriteBitcodeToFile(@"D:\Git\GitHub\Steadsoft\Syscode\TestSource\output.bc");
        }
    }
}
