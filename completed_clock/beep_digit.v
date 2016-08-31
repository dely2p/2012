module beep_digit(clk, clk_cnt, freq_out);
input clk, clk_cnt;
output freq_out;

wire cnt;
reg freq_out;
mod_6_counter U_beep_cnt (clk_cnt, rst, cnt);

always @(clk) begin

if(cnt == 0)
freq_out <= 638;
end

endmodule
