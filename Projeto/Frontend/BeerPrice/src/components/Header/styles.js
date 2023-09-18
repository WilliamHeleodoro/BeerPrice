import styled from "styled-components/native";
import fonts from "../../styles/fonts";

export const Container = styled.View`
  height: 70px;
  flex-direction: row;
  align-items: center;
  margin-top: 5%;
  margin-left: 3%;
`;

export const Icone = styled.Image``;

export const Title = styled.Text`
  color: #fff;
  font-size: 23px;
  font-family: ${fonts.fonts.titulo};
  margin-left: 3%;
  margin-right: 2%;
`;
