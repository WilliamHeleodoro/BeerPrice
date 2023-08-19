import React from "react";

import { Container, BannerItem, Title } from "./styleCervejas";


function ListaCervejas({ data, navigatePage}) {
  return (
    <Container activeOpacity={0.7} onPress={() => navigatePage(data)}>
      <BannerItem 
        resizeMethod="resize"
        source={{
          uri: data.imagem,
        }}
      />
      <Title numberOfLines={1}>{data.titulo}</Title>
      <Title>{data.quantidade} Unidade: {data.unidade}</Title>
      <Title>Id: {data.id}</Title>
    </Container>
  );
}

export default ListaCervejas;
