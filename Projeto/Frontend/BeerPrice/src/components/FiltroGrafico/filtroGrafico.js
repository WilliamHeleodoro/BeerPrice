import React from "react";
import { View, Text, TouchableOpacity, StyleSheet } from "react-native";

const Radio = ({
  options = [],
  horizontal = false,
  onChangeSelect,
  selected,
}) => {
  return (
    <View style={horizontal ? styles.horizontal : styles.vertical}>
      {options.map((opt, index) => (
        <TouchableOpacity
          key={index}
          onPress={() => onChangeSelect(opt, index)}
          style={styles.optContainer}
        >
          <View style={styles.outlineCircle}>
            {selected === index && <View style={styles.innerCircle} />}
          </View>
          <Text style={{ color: selected === index ? '#fff': '#777', padding: 4 }}>{opt}</Text>
        </TouchableOpacity>
      ))}
    </View>
  );
};

const styles = StyleSheet.create({
  horizontal: {
    padding: 8,
    marginLeft: 1,
    marginTop: 8,
    flexDirection: "row",
    alignItems: "center",
  },

  optContainer: {
    flexDirection: "row",
    alignItems: "center",
    padding: 8,
  },
  outlineCircle: {
    width: 20,
    height: 20,
    borderRadius: 10,
    borderColor: "#777",
    borderWidth: 2,
    justifyContent: "center",
    alignItems: "center",
    
  },
  innerCircle: {
    width: 10,
    height: 10,
    borderRadius: 5,
    backgroundColor: "#444",
  },
});
export default Radio;
