echo "Cropping $1 and exporting"
convert $1 -crop -2-6 +repage ../export/submarine_sillhouette_top.png