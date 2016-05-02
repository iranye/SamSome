use strict;

blocks0();

sub blocks0 {
	my $vb_lines = <<EOB;
   NextQuery = currSheet.Cells(%s, "K").Value
   NextSheet = currSheet.Cells(%s, "L").Value
   If Not (NextQuery = "") Then
        Call GetDataSheet(queryString:=NextQuery, sheet:=NextSheet)
   End If
   Sheets(NextSheet).Select

EOB

   my @na_params = (10, 16, 22, 28, 41, 47, 53, 59);
   my %na_params = map { $_ => 1 } @na_params;

   my $start = 36;
   my $end = 64;
   for (my $i = $start; $i <= $end; $i++) {
      unless (exists($na_params{$i}))
      {
         printf"$vb_lines", $i, $i;
      }
   }
}
sub lkup0 {
	my $limit=500;
	my $tab_name = "GVL-PTU-Actual";

	for (my $i = 2; $i < $limit; $i++) {
		# print("=IF(ISTEXT(VLOOKUP(G$i,'$tab_name'!N2:O5000,2, FALSE)))
		print("=IF(ISTEXT(VLOOKUP(G$i,'$tab_name'!N2:O5000,2, FALSE)),VLOOKUP(G$i,'$tab_name'!N2:O5000,2, FALSE),\"\")\n");
	}

	# =IF(ISTEXT(VLOOKUP(G499,'GRonGY-CUPS-Actual'!N2:O5000,2, FALSE)),VLOOKUP(G499,'GRonGY-CUPS-Actual'!N2:O5000,2, FALSE),"")
}

sub lkup1 {
# =IF(ISTEXT(VLOOKUP(A2,D:D,1,FALSE)),D1,IF(ISTEXT(VLOOKUP(A2,E:E,1,FALSE)),E1))
	my $limit=300;

	for (my $i = 2; $i < $limit; $i++) {
		print("=IF(ISTEXT(VLOOKUP(A$i,D:D,1,FALSE)),D1,IF(ISTEXT(VLOOKUP(A$i,E:E,1,FALSE)),E1,IF(ISTEXT(VLOOKUP(A$i,F:F,1,FALSE)),F1,IF(ISTEXT(VLOOKUP(A$i,G:G,1,FALSE)),G1,IF(ISTEXT(VLOOKUP(A$i,H:H,1,FALSE)),H1,IF(ISTEXT(VLOOKUP(A$i,I:I,1,FALSE)),I1,IF(ISTEXT(VLOOKUP(A$i,J:J,1,FALSE)),J1,IF(ISTEXT(VLOOKUP(A$i,K:K,1,FALSE)),K1,IF(ISTEXT(VLOOKUP(A$i,L:L,1,FALSE)),L1,IF(ISTEXT(VLOOKUP(A$i,M:M,1,FALSE)),M1,\"MANUAL\"))))))))))\n");
	}
}

