; ModuleID = 'output.bc'
source_filename = "test_call"

define i16 @add(i16 %0, i13 %1) {
  %3 = zext i13 %1 to i16
  %4 = add i16 %0, %3
  %5 = mul i16 %4, %3
  ret i16 %5
}
