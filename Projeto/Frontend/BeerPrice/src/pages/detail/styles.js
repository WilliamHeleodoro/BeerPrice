import { styled } from "styled-components/native";
import fonts from "../../styles/fonts";

export const Container = styled.View`
  flex: 1;
  background-color: #191a30;
`;

export const Title = styled.Text`
  color: #fff;
  font-size: 25px;
  text-align: center;
  font-family: ${fonts.fonts.titulo};
`;

export const Header = styled.View`
  flex-direction: row;
  position: absolute;
  top: 35px;
  width: 100%;
  display: flex;
  padding: 0 14px;
`;

export const HeaderButton = styled.TouchableOpacity`
  width: 46px;
  height: 46px;
  background-color: rgba(25, 26, 48, 0.8);
  border-radius: 23px;
  margin-top: 0.5%;
`;

export const Banner = styled.Image`
  width: 40%;
  height: 223px;
  border-radius: 8px;
`;

export const View = styled.View`
  top: 90px;
  flex-direction: row;
  margin-left: 10px;
  padding-right: 15px;
  border-radius: 8px;
  border: 1px;
  border-color: #808080;
`;

export const Precos = styled.FlatList``;

export const InfoProduto = styled.View`
  margin-top: 25%;
  align-items: left;
  margin-left: 10px;
  padding-right: 15px;
  border-radius: 8px;
  padding: 15px;

  border-bottom-color: #808080;
  border-bottom-width: 1px;
`;

export const InfoText = styled.Text`
  font-family: ${fonts.fonts.outros};
  color: #fff;
  font-family: ${fonts.fonts.outros};
  font-size: 14px;
  
`;


