import styled from "styled-components/native";
import fonts from "../../styles/fonts";

export const Container = styled.TouchableOpacity`
  width: 49%;
  align-items: center;
  padding: 15px;
  padding-left: 25px;
  margin-top: 15px;
`;

export const BannerItem = styled.Image`
  width: 160px;
  height: 160px;
  border-radius: 8px;
`;

export const Title = styled.Text`
  color: #fff;
  text-align: center;
  font-size: 11px;
  font-family: ${fonts.fonts.outros};
`;

export const Detalhes = styled.View`
  width: 160px;
  background-color: ${({ data }) => {
    if (data.mercado === "Brasão") {
      return "#800000";
    } else if (data.mercado === "Celeiro") {
      return "#701198";
    } else {
      return "#0a5c0a";
    }
  }};
  border-radius: 8px;
  padding-top: 3%;
  padding-bottom: 3%;
  margin-top: 3%;
`;
