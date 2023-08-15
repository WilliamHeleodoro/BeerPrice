import React from "react";
import { Text } from "react-native-paper";
import { View } from "react-native";

function DetalhesCerveja({ texto, quantidade, unidade }) {
  return (
    <View>
      <Text
        variant="labelLarge"
        style={{
          color: "#fff",
          fontSize: 14,
        }}
      >
        {texto}
      </Text>
      <Text
        variant="labelLarge"
        style={{
          color: "#fff",
          fontSize: 14,
        }}
      >
        Quantidade: {quantidade}
      </Text>
      <Text
        variant="labelLarge"
        style={{
          color: "#fff",
          fontSize: 14,
        }}
      >
        Unidade: {unidade}
      </Text>
    </View>
  );
}

export default DetalhesCerveja;
