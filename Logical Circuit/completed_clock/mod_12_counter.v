module mod_12_counter (clk, rst, cnt);
  input     		clk, rst;
  output	[3:0]	cnt;
  reg   	[3:0]	cnt;

	always @(posedge clk or posedge rst) begin
		if (rst) cnt <= 0;
		else begin
			if (cnt == 12) cnt <= 1;
			else	     cnt <= cnt + 1;
		end
	end

endmodule
