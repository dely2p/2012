module seg_dot(bcd_in, seg_data);

input bcd_in;
output [7:0] seg_data;

reg [7:0] seg_data;

always @ (bcd_in) begin
		case (bcd_in)
				1 : seg_data = 8'b1000_0000;
				default : seg_data = 8'b0000_0000;
	  endcase
end

endmodule
