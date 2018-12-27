
use strict; use warnings;
use Getopt::Std;

sub main {
  my %opt;
  getopts('f:o?h', \%opt)||die("Error: Incorrect command line options !!!\n");
  if (defined($opt{'f'})) {
    my $overwriteFlag = 0;
    if (defined($opt{'o'})) {
      $overwriteFlag = 1;
    }
    readAndReplace($opt{'f'}, $overwriteFlag);
  }
  else {
    printf "Usage: %s -f <file to update> [-o]\n", $0;
    printf "   -o: Overwrite file\n";
  }  
}

sub readAndReplace {
  my $file = shift;
  my $overwriteFlag = shift;
  my $tempFile = "temp.txt";
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  
  if ($overwriteFlag == 1) {
    open(OFH, ">$tempFile")||die("Error: Cannot open file $tempFile for output !!!\n\n");
  }
    
  while(<FH>) {
    chomp;
    my $line = $_;
    if ($line =~ /(.*)\[Test\](.*)/) {
      $line = sprintf "%s[TestMethod]%s", $1, $2;
    }
    if ($line =~ /(.*)\[Category(.*)\](.*)/) {
      $line = sprintf "%s[TestCategory%s\]%s", $1, $2, $3;
    }
    if ($line =~ /(.*)(RunSprocAsync\([^\)]*\));(.*)/) {
      $line = sprintf "%s%s.GetAwaiter().GetResult();%s", $1, $2, $3;
    }
    if ($line =~ /(.*)Task<TestRunTimings> task (.*)/) {
      $line = sprintf "%svar testRunTimings %s", $1, $2;      
    }
    if ($line =~ /TestRunTimings testRunTimings = await task;/) {
      next;
    }
    # TODO:  TestContext.CurrentContext.Test.MethodName
    # TestRunTimings testRunTimings = await task;
    printf "%s\r\n", $line;
    if ($overwriteFlag == 1) {
      print OFH sprintf "%s", $line;
    }
  }
}

main();