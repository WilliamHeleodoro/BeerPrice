import React from "react";

import { Container, BannerItem, Title } from "./styleCervejas";


function ListaCervejas({ data }) {
  return (
    <Container activeOpacity={0.7}>
      <BannerItem
        resizeMethod="resize"
        source={{
          uri: data.imagem,
        }}
      />
      <Title numberOfLines={1}>{data.titulo}</Title>
      <Title>{data.quantidade} Unidade: {data.unidade}</Title>
    </Container>
  );
}

export default ListaCervejas;
