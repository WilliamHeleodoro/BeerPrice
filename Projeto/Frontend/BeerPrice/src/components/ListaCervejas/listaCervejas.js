import React from "react";

import { Container, BannerItem, Title, RateContainer } from './styleCervejas';

function ListaCervejas(){
    return(
        <Container>
            <BannerItem
                source={{uri: 'https://img.freepik.com/fotos-gratis/foto-de-grande-angular-de-uma-unica-arvore-crescendo-sob-um-ceu-nublado-durante-um-por-do-sol-cercado-por-grama_181624-22807.jpg?w=2000'}}
            />
            <Title>Floresta</Title>
            <RateContainer></RateContainer>
        </Container>
    )
}

export default ListaCervejas;