echo "Cropping $1 and exporting"
convert $1 -crop -2-2 +repage ../export/$1