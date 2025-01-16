import React, { useState } from "react";
import { PayPalScriptProvider, PayPalButtons } from "@paypal/react-paypal-js";

const Pay = () => {
  const initialOptions = {
    "client-id": "AWvxrJl51eVxPbXUPf5N0RAbQc6LyZ71V2XLXv5zyzl0Z9eSbrZi8zpbnyyqaSs5JyyYxi7C1nV33WVa",
    currency: "USD",
    intent: "capture",
    components: "buttons",
    "data-sdk-integration-source": "developer-studio",
  };

  const [message, setMessage] = useState("");

  return (
    <div className="App">
      <PayPalScriptProvider options={initialOptions}>
        <PayPalButtons
          style={{
            shape: "rect",
            layout: "vertical",
            color: "gold",
            label: "paypal",
          }}
          createOrder={async (data, actions) => {
            try {
              const response = await fetch("/api/orders", {
                method: "POST",
                headers: {
                  "Content-Type": "application/json",
                },
                body: JSON.stringify({
                  // Thêm thông tin đơn hàng tại đây
                }),
              });
              const order = await response.json();
              console.log("Order created:", orderData);
              return order.id;
            } catch (error) {
              console.error("Error creating order:", error);
            }
          }}
          onApprove={async (data, actions) => {
            try {
              console.log("Payment approved, OrderID:", data.orderID);
              const response = await fetch(`/api/orders/${data.orderID}/capture`, {
                method: "POST"
              });
              const result = await response.json();
              console.log("Capture result:", result); 
              alert("Payment successful!"); 
            } catch (error) {
              console.error("Capture error:", error);
              alert("Payment failed!");
            }
          }}
        />
      </PayPalScriptProvider>
      {message && <p className="mt-4 text-center">{message}</p>}
    </div>
  );
};

export default Pay;