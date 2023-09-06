import React from "react";
import {Container, Title, Icone} from './styles';

function Header({ title }){
    return(
        <Container>
            <Icone
            source={require('../../../assets/cerveja.png')}
            />
            <Title>{title}</Title>
        </Container>
    )
}

export default Header;