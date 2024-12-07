import React from "react";
import { render, screen } from "@testing-library/react";
import App from "../App";

test("renders the main heading", () => {
  render(<App />);
  const headingElement = screen.getAllByText("Welcome to the Storefront");
  console.log(headingElement);
});
