module freq_div_N (clk_in, rst, clk_div_N);

  parameter   N=10;
  input		  rst, clk_in;
  output		clk_div_N;
  
  reg	[9:0]	cnt;
  reg		    clk_div_N;

always @(posedge clk_in or posedge rst) begin
	if (rst) begin
		cnt <= 0;
		clk_div_N <= 1;
	end
	else begin
		if (cnt == ((N>>1)-1)) begin
			cnt <=0;
			clk_div_N <= ~clk_div_N;
		end
		else	cnt <= cnt + 1;
	end
end

endmodule
