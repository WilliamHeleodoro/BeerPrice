import React from "react";
import {Container, Title} from './styles';

function Header({ title }){
    return(
        <Container>
            <Title>{title}</Title>
        </Container>
    )
}

export default Header;