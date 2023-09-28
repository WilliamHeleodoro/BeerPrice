import React from "react";
import { TouchableOpacity } from "react-native";
import { Container, Title, Icone } from "./styles";

function Header({ title, scrollToTop }) {
  return (
    <Container>
      <TouchableOpacity activeOpacity={0.5} onPress={scrollToTop}>
        <Icone source={require("../../../assets/cerveja.png")} />
      </TouchableOpacity>
      <Title>{title}</Title>
    </Container>
  );
}

export default Header;
