import styled from 'styled-components/native';
import fonts from '../../styles/fonts';


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

