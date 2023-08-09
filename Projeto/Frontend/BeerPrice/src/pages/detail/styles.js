import { styled } from "styled-components/native";

export const Container = styled.View`
  flex: 1;
  background-color: #191a30;
`;

export const Title = styled.Text`
 color: #fff;
    font-size: 30px;
    font-weight: bold;
    text-align: center;
`;

export const Header = styled.View`
flex-direction: row;
  z-index: 99;
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
  justify-content: center;
  align-items: center;
`;

export const Banner = styled.Image`
  width: 40%;
  height: 200px;
  top: 80px;
  border-radius: 8px;
  margin-left: 15px;
 
`;
