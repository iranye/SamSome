
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
    # readAndReplace($opt{'f'}, $overwriteFlag);
    # readAndReplaceStringBlocks($opt{'f'});
    ensureTestIntegrationAttribute($opt{'f'});
  }
  else {
    printf "Usage: %s -f <file to update> [-o]\n", $0;
    printf "   -o: Overwrite file\n";
  }  
}

sub ensureTestIntegrationAttribute {  
  my $file = shift;
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  
  my $scanningAttributes = 0;
  my $integrationAttributeFound = 0;
  my $i = 0;
  while(<FH>) {
    $i++;
    chomp;
    if ($scanningAttributes == 1) {
      if (!/[\s]+\[.*/) { # if we've reached a non-Attribute line and we haven't encountered the requisite Attibute line, add it
        $scanningAttributes = 0;
        if ($integrationAttributeFound == 0) {
          # print '[TestCategory("Integration")]\r\n"' ;
          printf "        %s\r\n", '[TestCategory("Integration")]';
        }
        $integrationAttributeFound = 0;
      }
      if (/[\s]+\[TestCategory\("Integration"\)\].*/) {
        $integrationAttributeFound = 1;
      }
    }
    if (/[\s]+\[TestMethod\].*/) {
      $scanningAttributes = 1;
      # if (/[\s]+\[(\S+)\].*/) {
        # printf "%s\r\n", $1;
      # }
    }
    # printf "[%s] scanningAttributes: %s got int attrib: %s\r\n", $i, $scanningAttributes, $integrationAttributeFound;
    printf "%s", $_;
  }  
}

sub readAndReplaceStringBlocks {
  my $file = shift;
  # my $tempFile = "temp.txt";
  open(FH, "<$file")||die("Error: Cannot open file $file for input !!!\n\n");
  
  # open(OFH, ">>$tempFile")||die("Error: Cannot open file $tempFile for output !!!\n\n");
    
  # my $replacementStr = "SPROC_NAME";
  my $codeBlock = '

        [TestMethod]
        [TestCategory("ZpTransactions")]
        public void Test_SPROC_NAME()
        {
            SprocName = "SPROC_NAME";
            var sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@paymentID", Value = 1, SqlDbType = SqlDbType.Int, IsNullable = false }
            };
            try
            {
                var testRunTimings = RunSprocAsync(sqlParameters).GetAwaiter().GetResult();
                UpdateTestData(testRunTimings, Util.GetCurrentMethodName());
            }
            catch (Exception ex)
            {
                UpdateTestData(FailedTestTimings, Util.GetCurrentMethodName());
                throw;
            }
        }
  ';
  
  # printf "%s\n", $codeBlock;
  
  while(<FH>) {
    chomp;
    (my $sprocName = $_) =~ s/([\S]+)\s*/$1/;
    # printf "'%s'\r\n", $sprocName;
    (my $newCode = $codeBlock) =~ s/(.*)SPROC_NAME(.*)/$1$sprocName$2/g;
    print $newCode;
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