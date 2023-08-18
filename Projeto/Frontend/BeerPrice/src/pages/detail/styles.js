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
  top: 5%;
  width: 100%;
  display: flex;
  padding: 0% 2%;
`;

export const HeaderButton = styled.TouchableOpacity`
  width: 12%;
  background-color: rgba(25, 26, 48, 0.8);
  margin-top: 0.5%;
`;

export const Banner = styled.Image`
  width: 40%;
  height: 100%;
  border-radius: 8px;
`;

export const View = styled.View`
 
  flex: 1;
  top: 22%;
  flex-direction: row;
  margin-left: 3%;
  margin-right: 2%;
  padding-right: 6%;
  border-radius: 8px;
  border-width: 1px;
  border-color: #808080;
`;

export const Precos = styled.FlatList``;

export const InfoProduto = styled.View`
  margin-top: 25%;
  align-items: left;
  margin-left: 3%;
  margin-right: 2%;
  padding-bottom: 2%;
  border-bottom-color: #808080;
  border-bottom-width: 1px;
`;

export const InfoText = styled.Text`
  font-family: ${fonts.fonts.outros};
  color: #fff;
  font-family: ${fonts.fonts.outros};
  font-size: 14px;
`;

export const Grafico = styled.View`
  flex: 1;
  margin-top: 2%;
`;

export const DetalhesGrafico = styled.View`
  flex-direction: row;
`;
